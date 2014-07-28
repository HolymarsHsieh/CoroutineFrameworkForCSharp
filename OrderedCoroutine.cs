
namespace ICS {

	public class OrderedCoroutine : ICoroutineBase
	{
		ICoroutineBase _former, _latter;
		public OrderedCoroutine( ICoroutineBase f, ICoroutineBase b )
		{
			_former = f; _latter = b;
		}
		
		override public bool finished { get { return _latter.finished; } }
			
		override public ICoroutineBase Update()
		{
			if(_former.finished)
			{
				_latter = _latter.Update();
				return _latter;
			}
			else
				_former = _former.Update();
			
			return this;
		}
	}

}