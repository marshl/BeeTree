using System;

public class AssertFailedException : Exception
{
    public AssertFailedException(  string message ) : base( message )
    {
		
    }
}

public static class Assert
{
    public static void IsTrue( bool condition, string message = "No Message" )
    {
        if ( condition == false )
        {
            throw new AssertFailedException( "Condition is False: " + message );
        }
    }
	
	public static void IsFalse( bool condition, string message = "No Message" )
	{
		if ( condition == true )
        {
            throw new AssertFailedException( "Condition is True: " + message );
        }	
	}

    public static void AreEqual( object expected, object actual, string message = "No Message" )
    {
        if ( actual.Equals(expected) == false )
        {
            throw new AssertFailedException(
                string.Format( "Assert.AreEqual failed. Expected value is {0} but the Actual value is {1}.", expected, actual ) + ": " + message );
        }
    }
    
    public static void IsNotNull( object obj, string message = "No Message" )
    {
        if ( obj == null )
        {
            throw new AssertFailedException( "Assert.IsNotNull failed: " + message );
        }
    }
	
	/*public static void AttributeMustBeSet( Object _object, Object _attribute )
	{
		if ( _attribute == null )
		{
			throw new AssertFailedException( _attribute.GetType().ToString() + " attached to " + _object.name.ToString() + " is null" );	
		}
	}*/
}

