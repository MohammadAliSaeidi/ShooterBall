using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace BallShooter.UI
{
	public class CustomSelectable : MonoBehaviour, ISelectHandler, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler, IPointerEnterHandler
	{
		public UnityEvent<CustomButtonState> e_OnSelectionStateChange;
		protected EventSystem eventSystem;

		public enum CustomButtonState
		{
			Normal,
			Highlighted,
			Pressed,
			Selected,
			Disabled
		}
		public CustomButtonState CurrentSelectionState { get; private set; }


		private void ChangeState(CustomButtonState newSelectionState)
		{
			switch(newSelectionState)
			{
				case CustomButtonState.Normal:
				break;

				case CustomButtonState.Highlighted:
				break;

				case CustomButtonState.Pressed:
				break;

				case CustomButtonState.Selected:
				break;

				case CustomButtonState.Disabled:
				break;
			}

			if(e_OnSelectionStateChange != null)
				e_OnSelectionStateChange.Invoke(newSelectionState);
		}


		public void OnSelect(BaseEventData eventData)
		{
			ChangeState(CustomButtonState.Selected);
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			ChangeState(CustomButtonState.Pressed);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if(eventSystem)
			{
				if(eventSystem.currentSelectedGameObject == this.gameObject)
				{
					ChangeState(CustomButtonState.Selected);
					return;
				}
			}
			ChangeState(CustomButtonState.Normal);
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			ChangeState(CustomButtonState.Highlighted);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			ChangeState(CustomButtonState.Highlighted);
		}

	}

}
