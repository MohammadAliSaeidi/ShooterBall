using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UISystem : MonoBehaviour {

	#region Vaiables
	[Header("System Events")]
	public UnityEvent OnSwitchedScreen = new UnityEvent();

	protected List<UIScreen> Screens = new List<UIScreen>();

	public UIScreen FirstScreen;
	public static float ShowTranstionDuration = 0.5f;
	public static float HideTranstionDuration = 0.5f;

	protected UIScreen currentScreen;
	protected UIScreen prevScreen;

	#region Properties
	public UIScreen CurrentScreen { get { return currentScreen; } }
	public UIScreen PrevScreen { get { return prevScreen; } }
	#endregion

	#endregion

	#region Methods

	private void Awake () {
	}

	private void Start () {
		ShowFirstScreen();
	}

	public void SwitchScreens (UIScreen screen) {
		if(screen) {
			if(currentScreen) {
				currentScreen.CloseScreen();
				prevScreen = currentScreen;
			}

			currentScreen = screen;
			currentScreen.gameObject.SetActive(true);
			currentScreen.ShowScreen();

			if(OnSwitchedScreen != null)
				OnSwitchedScreen.Invoke();
		}
	}

	public virtual void ShowFirstScreen () {
		if(FirstScreen) {
			SwitchScreens(FirstScreen);
		}
	}

	public virtual void GoToPrevScreen () {
		if(CurrentScreen.OverridePrevScreen == true && CurrentScreen.PrevScreen != null)
			SwitchScreens(CurrentScreen.PrevScreen);
		else if(PrevScreen)
			SwitchScreens(PrevScreen);
		else Debug.LogError("Null PrevPage!");
		//if(PrevScreen) {
		//	if(CurrentScreen.OverridePrevScreen)
		//		SwitchScreens(CurrentScreen.PrevScreen);
		//	else
		//		SwitchScreens(PrevScreen);
		//}
	}

	public virtual void CloseAllScreens () {
		Screens.ForEach(i => {
			i.gameObject.SetActive(true);
			i.CloseScreen();
		});
	}
	#endregion
}
