using System;

namespace SampleApi.Web.Infrastructure
{
  public class Envelop<T>
  {
    public T Result { get; }
    public string ErrorMessage { get; }
    public DateTime TimeGenerated { get; }

    protected internal Envelop(T result, string errorMessage)
    {
      Result = result;
      ErrorMessage = errorMessage;
      TimeGenerated = DateTime.UtcNow;
    }
  }

  public class Envelop : Envelop<string>
  {
    protected Envelop(string errorMessage) : base(null, errorMessage)
    {
    }

    public static Envelop<T> Ok<T>(T result)
    {
      return new Envelop<T>(result, null);
    }

    public static Envelop Ok()
    {
      return new Envelop(null);
    }

    public static Envelop Error(string errorMessage)
    {
      return new Envelop(errorMessage);
    }
  }
}
