using UnityEngine;

namespace BallShooter.Player
{
	public class CrosshairManager : MonoBehaviour
	{
		[Header("CrossHair")]
		public CrossHairMaster crossHair;

		private void Start()
		{

		}

		public void InitCrosshair(Gun gun)
		{
			if (gun)
			{
				crossHair.gameObject.SetActive(true);
				foreach (var s in crossHair.sliders)
				{
					s.maxValue = gun.Specifics.recoilInfo.VerticalRecoil * 10;
				}
			}
			else
			{
				crossHair.gameObject.SetActive(false);
			}
		}

		public void HandleCrossHair(float additiveValue)
		{
			foreach (var s in crossHair.sliders)
			{
				s.value += additiveValue;
			}
		}
	}
}