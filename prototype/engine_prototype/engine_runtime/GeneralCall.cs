namespace EngineRuntime;

public static class GeneralCall
{
    public static void InitComponentCallback()
    {
        unsafe
        {
            InternalComponent.SetGlobalComponentCallback(
                &InternalComponent.ComponentStart, 
                &InternalComponent.ComponentUpdate, 
                &InternalComponent.ComponentDestroy);   
        }
    }
    
    public static void UpdateAllGameObject()
    {
        InternalGeneralCall.UpdateAllGameObject();
    }

    public static void DestroyAllGameObject()
    {
        InternalGeneralCall.DestroyAllGameObject();
    }
}