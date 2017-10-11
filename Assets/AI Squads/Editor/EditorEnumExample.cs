using UnityEditor;

public class EditorEnumExample : EditorWindow
{
    public enum Example
    {
        Option_One = 1, //bits: 0000 0001
        Option_Two = 2,  //bits: 0000 0010
        Option_Three = 4     //bits: 0000 0100
    }

    Example staticFlagMask = 0;


    [MenuItem("Examples/Mask Field Usage")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        EditorEnumExample window = (EditorEnumExample)EditorWindow.GetWindow(typeof(EditorEnumExample));
        window.Show();
    }

    void OnGUI()
    {
        staticFlagMask = (Example)EditorGUILayout.EnumMaskField("Static Flags", staticFlagMask);

        // If "Everything" is set, force Unity to unset the extra bits by iterating through them
        if ((int)staticFlagMask < 0)
        {
            int bits = 0;
            foreach (var enumValue in System.Enum.GetValues(typeof(Example)))
            {
                int checkBit = (int)staticFlagMask & (int)enumValue;
                if (checkBit != 0)
                    bits |= (int)enumValue;
            }

            staticFlagMask = (Example)bits;
        }
    }
}