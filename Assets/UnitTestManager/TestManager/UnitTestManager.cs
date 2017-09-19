using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

using UnityEngine;

public class TestItem
{
    public string name;
    public string message;
    public bool? success;
	public float timeTaken;
}

public class UnitTestManager
{
    public List<TestItem> testItems { get; private set; }
    
    public bool IsRunning { get; set; }

    public UnitTestManager()
    {
        testItems = new List<TestItem>();
    }
    
    public void RunTests()
    {
        if (IsRunning)
        {
            return;
        }
            
        testItems.Clear();
       
        IsRunning = true;

        var assembly = Assembly.GetCallingAssembly();
        Type[] types = assembly.GetTypes();

        // Find all the test classes
        //
        foreach (Type type in types)
        {
            if (type.IsDefined(typeof(TestFixtureAttribute), false))
            {
				
                object fixtureInstance;// = Activator.CreateInstance(type); 
				GameObject obj = null;
				if ( type.IsSubclassOf( typeof(MonoBehaviour) ) )
				{
					obj = new GameObject();
					fixtureInstance = obj.AddComponent( type );
				}
				else
				{
					fixtureInstance = Activator.CreateInstance( type ); 
				}
				
                MethodInfo[] methods = type.GetMethods();
                MethodInfo setUpMethod = methods.FirstOrDefault(x => x.IsDefined(typeof(SetUpAttribute), false));
                MethodInfo tearDownMethod = methods.FirstOrDefault(x => x.IsDefined(typeof(TearDownAttribute), false));
                foreach ( MethodInfo testMethod in methods )
                {
                    if ( testMethod.IsDefined(typeof(TestAttribute), false) == false)
                    {
						continue;	
					}
					
					float timeBeforeTest = Time.realtimeSinceStartup;
					
                    // Add the item to the list for UI purposes
                   	// testItems.Add(new TestItem { Name = type.Name + "::" + testMethod.Name });
  					TestItem testItem = new TestItem();
					testItem.name = (type.Name + "::" + testMethod.Name);
					this.testItems.Add( testItem );
                    testItem.success = true;

                    try
                    {
                        // First run setup for the test if available
                        if (setUpMethod != null)
                        {
                            setUpMethod.Invoke(fixtureInstance, null);
                        }
                    }
                    catch (Exception e)
                    {
                        testItem.success = false;
                        testItem.message = e.ToString();
                    }
  
                    
                    // If the setup was successful, run the test method, ignoring expected exceptions
                    //
                    if (testItem.success.Value)
                    {
                        try
                        {
                            testMethod.Invoke(fixtureInstance, null);
                        }
                        catch (Exception e)
                        {
                            if (testMethod.IsDefined(typeof(ExpectedException), false))
                            {
                                var expectedEx = testMethod.GetCustomAttributes(typeof(ExpectedException), false)[0] as ExpectedException;
                                if (e.InnerException == null || expectedEx.ExceptionType != e.InnerException.GetType())
                                {
                                    testItem.success = false;
                                    testItem.message = e.ToString();
                                }
                            }
                            else
                            {
                                testItem.success = false;
                                testItem.message = e.ToString();
                            }
                        }
                    }
  
                    try
                    {
                        // Finally clean up regardless of previous results
                        //
                        if (tearDownMethod != null)
                        {
                            tearDownMethod.Invoke(fixtureInstance, null);
                        }
                    }
                    catch (Exception e)
                    {
                        testItem.success = false;
                        testItem.message = e.ToString();
                    }
					
					testItem.timeTaken = Time.realtimeSinceStartup - timeBeforeTest;
                }
				
				if ( obj != null )
				{
					GameObject.DestroyImmediate( obj );
				}
            }
        }
        
        IsRunning = false;
    }
	
	public void DestroyTests()
	{
		this.testItems.Clear();	
	}
}
