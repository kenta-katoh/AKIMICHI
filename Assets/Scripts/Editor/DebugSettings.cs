using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

static class DebugSettings
{
    [SettingsProvider]
    public static SettingsProvider CreateExampleProvider()
    {
        // 第二引数をSettingsScope.Projectに変更した
        var provider = new SettingsProvider("AKIMICHI/", SettingsScope.Project)
        {
            label = "DebugSettings",
            guiHandler = searchContext => EditorGUILayout.Popup(
                label: new GUIContent("Popup"),
                selectedIndex: m_PopupIndex,
                displayedOptions: m_PopupDisplayOptions
                ),
            keywords = new HashSet<string>(new[] { "プレイヤー" })
        };

        return provider;
    }

    private static GUIContent[] m_PopupDisplayOptions = new[] {
                                                                new GUIContent("1"),
                                                                new GUIContent("2"),
                                                                new GUIContent("3"),
                                                                new GUIContent("4"),
                                                               };
    private static int m_PopupIndex;
}
