//from unity3d script reference

// Render scene from a given point into a static cube map.
// Place this script in Editor folder of your project.
// Then use the cubemap with one of Reflective shaders!
class RenderCubemapWizard extends ScriptableWizard {
    var renderFromPosition : Transform;
    var cubemap : Cubemap;
    
    function OnWizardUpdate () {
        helpString = "Select transform to render from and cubemap to render into";
        isValid = (renderFromPosition != null) && (cubemap != null);
    }
    
    function OnWizardCreate () {
        // create temporary camera for rendering
        var go = new GameObject( "CubemapCamera", Camera );
        // place it on the object
        go.transform.position = renderFromPosition.position;
        go.transform.rotation = Quaternion.identity;

        // render into cubemap        
        go.GetComponent.<Camera>().RenderToCubemap( cubemap );
        
        // destroy temporary camera
        DestroyImmediate( go );
    }
    
    @MenuItem("GameObject/Render into Cubemap")
    static function RenderCubemap () {
        ScriptableWizard.DisplayWizard.<RenderCubemapWizard>(
            "Render cubemap", "Render!");
    }
}