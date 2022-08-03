using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BallShooter.UI
{
	[RequireComponent(typeof(Image))]
	[RequireComponent(typeof(Button))]
	public class CustomButton : CustomSelectable
	{
		private Button button;
		private Image image;

		[Header("Sprite Swap")]
		public bool SpriteSwap = false;
		private Sprite mainSprite;
		public Sprite HighlightedSprite;
		public Sprite PressedSprite;
		public Sprite SelectedSprite;
		public Sprite DisabledSprite;

		private void Awake()
		{
			button = GetComponent<Button>();
			image = GetComponent<Image>();
			eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
		}

		private void Start()
		{
			if(SpriteSwap)
			{
				mainSprite = image.sprite;
				e_OnSelectionStateChange.AddListener(delegate
				{
					SwapSprite();
				});
			}
		}

		private void SwapSprite()
		{
			switch(CurrentSelectionState)
			{
				case CustomButtonState.Normal:
				image.sprite = mainSprite;
				break;

				case CustomButtonState.Highlighted:
				image.sprite = HighlightedSprite;
				break;

				case CustomButtonState.Pressed:
				image.sprite = PressedSprite;
				break;

				case CustomButtonState.Selected:
				image.sprite = SelectedSprite;
				break;

				case CustomButtonState.Disabled:
				image.sprite = DisabledSprite;
				break;
			}
		}
	}
}