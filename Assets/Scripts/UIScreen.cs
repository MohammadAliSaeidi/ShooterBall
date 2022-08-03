using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Diagnostics;
using System.Windows.Markup;
using UnityEngine.UIElements;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CanvasGroup))]
public class UIScreen : MonoBehaviour {
	#region Vaiables

	//[SerializeField] protected bool allowOpenPrevScreen = true;
	//public bool IsPrevScreenAllowed {
	//	get => allowOpenPrevScreen;
	//	protected set => allowOpenPrevScreen = value;
	//}

	public bool ShowTheScreen = false;
	public bool CloseTheScreen = false;

	[Header("Overrides")]
	[SerializeField] protected bool overridePrevScreen = false; // description : if checked you should override previous page (UIScreen)
	[SerializeField] protected UIScreen prevScreen;
	public bool OverridePrevScreen {
		get => overridePrevScreen;
		set => overridePrevScreen = value;
	}
	public UIScreen PrevScreen {
		get => prevScreen;
		set => prevScreen = value;
	}

	[SerializeField] protected bool overrideShowTranstionDuration = false;
	public bool OverrideShowTranstionDuration {
		get => overrideShowTranstionDuration;
		protected set => overrideShowTranstionDuration = value;
	}
	public float ShowTranstionDuration;

	[SerializeField] protected bool overrideHideTranstionDuration = false;
	public bool OverrideHideTranstionDuration {
		get => overrideHideTranstionDuration;
		protected set => overrideHideTranstionDuration = value;
	}
	public float HideTranstionDuration;

	public bool DelayBeforeScreenSwitch = true;

	[Header("Screen Events")]
	public UnityEvent OnScreenStart = new UnityEvent();
	public UnityEvent OnScreenClose = new UnityEvent();

	[HideInInspector]
	public Animator animator;
	#endregion

	#region Unity Methods
	private void Awake() {
		animator = GetComponent<Animator>();
	}

	private void Update() {
		if(ShowTheScreen) {
			ShowScreen();
			ShowTheScreen = false;
		}
		if(CloseTheScreen) {
			CloseScreen();
			CloseTheScreen = false;
		}
	}
	#endregion

	#region Methods
	public void ShowScreen() {
		animator.SetFloat("ShowTranstionDuration", OverrideShowTranstionDuration ? 1 / ShowTranstionDuration : 1 / UISystem.ShowTranstionDuration);
		animator.SetFloat("HideTranstionDuration", OverrideHideTranstionDuration ? 1 / HideTranstionDuration : 1 / UISystem.HideTranstionDuration);
		StartCoroutine(Co_ShowScreen());
	}

	IEnumerator Co_ShowScreen() {
		float delay = 0;
		if(DelayBeforeScreenSwitch)
			delay = UISystem.ShowTranstionDuration / 2;
		yield return new WaitForSeconds(delay);
		if(OnScreenStart != null) {
			OnScreenStart.Invoke();
		}
		if(animator) {
			animator.SetTrigger("Show");
		}
	}

	public void CloseScreen() {
		if(OnScreenClose != null) {
			OnScreenClose.Invoke();
		}
		HandleAnimator("Hide");
	}

	void HandleAnimator(string aTrigger) {
		if(animator) {
			animator.SetTrigger(aTrigger);
		}
	}
	#endregion
}
