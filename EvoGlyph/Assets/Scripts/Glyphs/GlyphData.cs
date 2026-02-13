using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "GlyphData", menuName = "Glyphs Patterns/GlyphData")]
public class GlyphData : ScriptableObject
{
    [Header("Elemental Attributes")]
    [SerializeField] public ElementType Element;

    [Header("Grid Settings")]
    public int width = 4;
    public int size = 16;
    private BitArray bits;
    private BitArray seqs;
    [SerializeField] public bool[] glyphPattern;
    [SerializeField] public int[] glyphSequence;
    [SerializeField] public GameObject spellPrefab;
    private void Awake()
    {
        bits = new BitArray(glyphPattern);
        seqs = new BitArray(glyphPattern);
    }
}

[CustomEditor(typeof(GlyphData))]
public class GlyphDataEditor : Editor
{
    SerializedProperty element;
    SerializedProperty spellPrefab;
    SerializedProperty bools;
    SerializedProperty sequences;
    SerializedProperty width;
    SerializedProperty size;

    public void OnEnable()
    {
        element = this.serializedObject.FindProperty("Element");
        spellPrefab = this.serializedObject.FindProperty("spellPrefab");
        bools = this.serializedObject.FindProperty("glyphPattern");
        sequences = this.serializedObject.FindProperty("glyphSequence");
        width = this.serializedObject.FindProperty ("width");
        size = this.serializedObject.FindProperty ("size");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(element);
        EditorGUILayout.PropertyField(spellPrefab);
        GUILayout.Space(10);

        CollisionDataInspector.Show(bools, size, width);
        GUILayout.Space(10);

        SequenceEditor.Show(sequences, size, width);
        GUILayout.Space(5);

        serializedObject.ApplyModifiedProperties();
    }
}

public class CollisionDataInspector : MonoBehaviour
{
    public static void Show(SerializedProperty bools, SerializedProperty size, SerializedProperty width)
    {
        EditorGUILayout.LabelField("Glyph Board Grid", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(size);
        EditorGUILayout.PropertyField(width);

        bools.arraySize = size.intValue; //control the size of the array via the exposed size variable
        if (size.intValue < 1) { size.intValue = 1; }
        if (width.intValue < 1) { width.intValue = 1; }

        //make the text of the Selection Grid buttons correspond to the contents of the array
        string[] buttonText = new string[bools.arraySize];
        for (int i = 0; i < buttonText.Length; i++)
        {
            buttonText[i] = bools.GetArrayElementAtIndex(i).boolValue ? "*" : " ";
        }

        //padding
        GUILayout.Space(3);
        GUILayout.BeginHorizontal();
        GUILayout.Space(15);

        int selected = GUILayout.SelectionGrid(-1, buttonText, width.intValue, new GUILayoutOption[] { GUILayout.Width(21 * width.intValue), GUILayout.Height(21 * ((size.intValue + width.intValue - 1) / width.intValue)) });
        GUILayout.EndHorizontal();

        //padding
        GUILayout.Space(3);
        GUILayout.BeginHorizontal();
        GUILayout.Space(15);
        //empty and fill buttons set all values in the array to false/true respectively
        if (GUILayout.Button("Empty", new GUILayoutOption[] { GUILayout.Width(50) }))
        {
            for (int i = 0; i < bools.arraySize; i++) { bools.GetArrayElementAtIndex(i).boolValue = false; }
        }
        if (GUILayout.Button("Fill", new GUILayoutOption[] { GUILayout.Width(30) }))
        {
            for (int i = 0; i < bools.arraySize; i++) { bools.GetArrayElementAtIndex(i).boolValue = true; }
        }
        GUILayout.EndHorizontal();

        //when a button is prressed, check which one it is, toggle the boolean value at that position in the array
        if (selected != -1)
        {
            bools.GetArrayElementAtIndex(selected).boolValue = !bools.GetArrayElementAtIndex(selected).boolValue;
            selected = -1;
        }
        EditorGUI.indentLevel--;
    }
}
public class SequenceEditor :MonoBehaviour
{
    public static void Show(SerializedProperty sequences, SerializedProperty size, SerializedProperty width)
    {
        EditorGUILayout.LabelField("Sequence Mapper", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        //EditorGUILayout.PropertyField(size);
        //EditorGUILayout.PropertyField(width);

        if (size.intValue < 1) size.intValue = 1;
        if (width.intValue < 1) width.intValue = 1;

        sequences.arraySize = size.intValue;

        int height = (size.intValue + width.intValue - 1) / width.intValue;

        GUILayout.Space(3);

        for (int row = 0; row < height; row++)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(15);

            for (int col = 0; col < width.intValue; col++)
            {
                int index = row * width.intValue + col;
                if (index >= size.intValue)
                    break;

                SerializedProperty elem = sequences.GetArrayElementAtIndex(index);
                int val = elem.intValue;

                int newVal = EditorGUILayout.IntField(val, GUILayout.Width(50));

                // Optional: Clamp or validate newVal here
                if (newVal != val)
                    elem.intValue = newVal;
            }

            GUILayout.EndHorizontal();
        }

        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        GUILayout.Space(15);

        if (GUILayout.Button("Clear", GUILayout.Width(50)))
        {
            for (int i = 0; i < sequences.arraySize; i++)
                sequences.GetArrayElementAtIndex(i).intValue = 0;
        }

        if (GUILayout.Button("Fill Ascending", GUILayout.Width(100)))
        {
            for (int i = 0; i < sequences.arraySize; i++)
                sequences.GetArrayElementAtIndex(i).intValue = i + 1;
        }

        GUILayout.EndHorizontal();

        EditorGUI.indentLevel--;
    }

}