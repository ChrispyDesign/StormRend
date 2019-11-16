namespace StormRend.Units
{
	public class InAnimateUnit : Unit
	{
		/// <summary>
		/// Inanimate units are would just simply deactivate on "death"	
		/// This should be triggered by an animation event.
		/// </summary>
		public override void Die()
		{
			base.Die();
			gameObject.SetActive(false);
		}
	}
}