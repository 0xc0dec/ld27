public class FadeIn : FadeOut
{
	protected override void Awake()
	{
		base.Awake();
		_inverseAlpha = true;
	}
}
