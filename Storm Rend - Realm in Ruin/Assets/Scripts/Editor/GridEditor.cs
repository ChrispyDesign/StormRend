using UnityEngine;
using UnityEditor;


//[CustomEditor(typeof(LevelNodes))]
//public class LevelEditor : Editor
//{
//    NodeType m_nodeType;
//    public bool showLevels = true;

//    public override void OnInspectorGUI()
//    {
//        LevelNodes levels = (LevelNodes)target;
//        EditorGUILayout.Space();
        
//        showLevels = EditorGUILayout.Foldout(showLevels, "Levels (" + levels.m_nodes.Length + ")");
//        if (showLevels)
//        {
//            EditorGUI.indentLevel++;
//            for (ushort i = 0; i < levels.m_nodes.Length; i++)
//            {

//                //levels.allLevels[i].showBoard = EditorGUILayout.Foldout(levels.allLevels[i].showBoard, "Board");
//                //if (levels.allLevels[i].showBoard)
//                //{
//                    EditorGUI.indentLevel = 0;

//                    GUIStyle tableStyle = new GUIStyle("box");
//                    tableStyle.padding = new RectOffset(10, 10, 10, 10);
//                    tableStyle.margin.left = 32;

//                    GUIStyle headerColumnStyle = new GUIStyle();
//                    headerColumnStyle.fixedWidth = 35;

//                    GUIStyle columnStyle = new GUIStyle();
//                    columnStyle.fixedWidth = 65;

//                    GUIStyle rowStyle = new GUIStyle();
//                    rowStyle.fixedHeight = 25;

//                    GUIStyle rowHeaderStyle = new GUIStyle();
//                    rowHeaderStyle.fixedWidth = columnStyle.fixedWidth - 1;

//                    GUIStyle columnHeaderStyle = new GUIStyle();
//                    columnHeaderStyle.fixedWidth = 30;
//                    columnHeaderStyle.fixedHeight = 25.5f;

//                    GUIStyle columnLabelStyle = new GUIStyle();
//                    columnLabelStyle.fixedWidth = rowHeaderStyle.fixedWidth - 6;
//                    columnLabelStyle.alignment = TextAnchor.MiddleCenter;
//                    columnLabelStyle.fontStyle = FontStyle.Bold;

//                    GUIStyle cornerLabelStyle = new GUIStyle();
//                    cornerLabelStyle.fixedWidth = 42;
//                    cornerLabelStyle.alignment = TextAnchor.MiddleRight;
//                    cornerLabelStyle.fontStyle = FontStyle.BoldAndItalic;
//                    cornerLabelStyle.fontSize = 14;
//                    cornerLabelStyle.padding.top = -5;

//                    GUIStyle rowLabelStyle = new GUIStyle();
//                    rowLabelStyle.fixedWidth = 25;
//                    rowLabelStyle.alignment = TextAnchor.MiddleRight;
//                    rowLabelStyle.fontStyle = FontStyle.Bold;

//                    GUIStyle enumStyle = new GUIStyle("popup");
//                    rowStyle.fixedWidth = 65;

//                    EditorGUILayout.BeginHorizontal(tableStyle);
//                    for (int x = -1; x < levels.column; x++)
//                    {
//                        EditorGUILayout.BeginVertical((x == -1) ? headerColumnStyle : columnStyle);
//                        for (int y = -1; y < levels.rows; y++)
//                        {
//                            if (x == -1 && y == -1)
//                            {
//                                EditorGUILayout.BeginVertical(rowHeaderStyle);
//                                EditorGUILayout.LabelField("[X,Y]", cornerLabelStyle);
//                                EditorGUILayout.EndHorizontal();
//                            }
//                            else if (x == -1)
//                            {
//                                EditorGUILayout.BeginVertical(columnHeaderStyle);
//                                EditorGUILayout.LabelField(y.ToString(), rowLabelStyle);
//                                EditorGUILayout.EndHorizontal();
//                            }
//                            else if (y == -1)
//                            {
//                                EditorGUILayout.BeginVertical(rowHeaderStyle);
//                                EditorGUILayout.LabelField(x.ToString(), columnLabelStyle);
//                                EditorGUILayout.EndHorizontal();
//                            }

//                        if (x >= 0 && y >= 0)
//                        {
//                            EditorGUILayout.BeginHorizontal(rowStyle);
//                            levels.m_nodes[x, y].m_nodeType = (NodeType)EditorGUILayout.EnumPopup(levels.m_nodes[x, y].m_nodeType, enumStyle);
//                            EditorGUILayout.EndHorizontal();
//                        }
//                    }
//                        EditorGUILayout.EndVertical();
//                    }
//                    EditorGUILayout.EndHorizontal();

//                //}

//            }
//        }
//    }
//}