using System.Collections;

namespace ICS {

	public abstract class ICoroutineBase
	{
		public abstract bool finished { get; }
		public abstract ICoroutineBase Update();
		
		public static GroupedCoroutine operator +(ICoroutineBase left, ICoroutineBase right)
		{
			if(left is GroupedCoroutine)
				return ((GroupedCoroutine)left) + right;
			
			if(right is GroupedCoroutine)
				return ((GroupedCoroutine)right) + left;
				
			return new GroupedCoroutine(left, right);
		}
		
	    public static GroupedCoroutine operator+(ICoroutineBase left, IEnumerator right)
	    {
	        return left + new Predefined.LogicCoroutine(right);
	    }
	    
	    public static GroupedCoroutine operator+(IEnumerator left, ICoroutineBase right)
	    {
	        return right + new Predefined.LogicCoroutine(left);
	    }
	 
	    public static OrderedCoroutine operator/(ICoroutineBase left, IEnumerator right)
	    {
	        return left / new Predefined.LogicCoroutine(right);
	    }
	    
	    public static OrderedCoroutine operator/(IEnumerator left, ICoroutineBase right)
	    {
	        return right / new Predefined.LogicCoroutine(left);
	    }
	        
		public static OrderedCoroutine operator/(ICoroutineBase left, ICoroutineBase right)
		{
			return new OrderedCoroutine(left, right);
		}
	}

	public class CoroutineExecuter
	{
		ICoroutineBase coroutine = null;
		
		public CoroutineExecuter() { coroutine = new GroupedCoroutine(); }
		
	    public void clear() { coroutine = new GroupedCoroutine(); }
		public bool finished { get { return coroutine.finished; } }
		public void Update()
		{
			ICoroutineBase temp = coroutine.Update();
			coroutine = temp;
		}
		
		public void Run(ICoroutineBase c) { coroutine += c; }
		
		public void Run(IEnumerator block) { coroutine += new Predefined.LogicCoroutine(block); }
		
		public void RunAfter(ICoroutineBase c) { coroutine /= c; }
		
		public void RunAfter(IEnumerator block) { coroutine /= new Predefined.LogicCoroutine(block); }
		
		public void RunBefore(ICoroutineBase c) { coroutine = c / coroutine; }
		
		public void RunBefore(IEnumerator block) { coroutine = new Predefined.LogicCoroutine(block) / coroutine; }
		
		public static CoroutineExecuter operator +(CoroutineExecuter left, ICoroutineBase right)
		{ left.Run(right); return left; }
		
		public static CoroutineExecuter operator +(CoroutineExecuter left, IEnumerator right)
		{ left.Run(right); return left; }
		
		public static CoroutineExecuter operator /(CoroutineExecuter left, ICoroutineBase right)
		{ left.RunAfter(right); return left; }
		
		public static CoroutineExecuter operator /(CoroutineExecuter left, IEnumerator right)
		{ left.RunAfter(right); return left; }

	}
	
}