using System;
using System.Collections.Generic;
using System.Linq;

/*
    This snippet is a bit open-ended ...
	
    How familiar are you with Generics?
	
	If you aren't familiar, then research and explain as much as possible
	
	If you are then can you propose a different approach?
	Particularly, an approach that doesn't require
	explicit reference to the type when calling the
	static Success/Failure constructor.
*/

namespace ENGenerics
{
    class Program
    {
        static void Main(string[] args)
        {
            var note = new MyNote { Note = "Foobaz" };

			//Refactoring to replace var with an explicit type
			//EnResult<MyNote> result = EnResult<MyNote>.Failure(note, "this", "that", "the other thing");

			//Target-typed new-expressions C# 9.0 - an expression gets the type from the context it is used in
			//the new expression gets the type from the context, 
			//which means you don’t have to specify the target-type explicitly to call a constructor.
			//EnResult<MyNote> result = new().Failure(note, "this", "that", "the other thing");
			//var vs target-typed new expressiong - the C# compiler produces exactly the same Intermediate Language code
			
			var result = EnResult<MyNote>.Failure(note, "this", "that", "the other thing");

			Console.WriteLine(result.Errors.Count());
        }

		public class MyNote
		{
			public string Note { get; set; }			
        }
    }

	// Q. what's the value of making this Generic
	// A. We can write generalized reusable code that is type-safe, yet works with any data type 

	// Q. what would the alternative be if we didn't use Generics
	//boxing/unboxing (where .net has to convert value types to reference types or vice-versa) or casting from objects to the required reference type.

	public class EnResult<T> : IEnResult<T>
	{
		public bool IsSuccess { get; private set; }
		public List<string> Errors { get; private set; } = new List<string>();
		public T Data { get; private set; }

		public static EnResult<T> Success(T data)
		{
			return new EnResult<T>(data) { IsSuccess = true };
		}

		// Q. can you propose alternatives to the parsms idiom
		//    for setting error messages?

		//params enables methods to receive variable numbers of parameters/arguments....0 to infinity # of Parameters 
		
		//public static EnResult<T> Failure(T data, string[] errors) // without param
		//var result = EnResult<MyNote>.Failure(note, new string[] { "this", "that", "the other thing" }); //can call the method with an array as a parameter 

		public static EnResult<T> Failure(T data, params string[] errors)
		{
			var x = new EnResult<T>(data) { IsSuccess = false };
			x.Errors.AddRange(errors);
			return x;
		}

		private EnResult(T data)
		{
			this.Data = data;
		}
	}

	public interface IEnResult
	{
		bool IsSuccess { get; }
		List<string> Errors { get; }
	}

	public interface IEnResult<T> : IEnResult
	{
		T Data { get; }
	}
}
