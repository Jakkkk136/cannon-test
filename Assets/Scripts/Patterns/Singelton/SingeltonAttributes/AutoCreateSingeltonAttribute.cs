using System;

namespace Patterns.Singelton.SingeltonAttributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class AutoCreateSingeltonAttribute : Attribute
	{
	}
}