using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace gtm
{
    /// <summary>
    /// 
    /// </summary>
    public class MaterialTexReplaceEditor : EditorWindow
    {
        /// <summary>
        /// 
        /// </summary>
        [MenuItem("gtm/Scene/²ÄÖÊÌùÍ¼Ìæ»»±à¼­Æ÷", false, 300)]
        static void OnOpen()
        {
            MaterialTexReplaceEditor window = new MaterialTexReplaceEditor();
            window.titleContent = new GUIContent("²ÄÖÊÌùÍ¼Ìæ»»±à¼­Æ÷");
            window.Show();
        }

        /// <summary>
        /// 
        /// </summary>
        Texture m_SrcTex;

        /// <summary>
        /// 
        /// </summary>
        Texture m_DstTex;

        /// <summary>
        /// 
        /// </summary>
        void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            m_SrcTex = EditorGUILayout.ObjectField("Ô´ÌùÍ¼", m_SrcTex, typeof(Texture), false) as Texture;
            m_DstTex = EditorGUILayout.ObjectField("Ä¿±êÌùÍ¼", m_DstTex, typeof(Texture), false) as Texture;

            if (GUILayout.Button("Ìæ»»ÌùÍ¼"))
            {
                ReplaceAllTexture();
            }

            EditorGUILayout.EndHorizontal();
        }

        public static List<Material> GetAllMaterials()
        {
            List<Material> mats = new List<Material>();
            string[] assetPaths = AssetDatabase.GetAllAssetPaths();

            foreach (string assetPath in assetPaths)
            {
                if (!assetPath.EndsWith(".mat"))
                {
                    continue;
                }

                Material mat = AssetDatabase.LoadAssetAtPath<Material>(assetPath);
                if (mat != null)
                {
                    mats.Add(mat);
                }
            }

            return mats;
        }

        /// <summary>
        /// 
        /// </summary>
        void ReplaceAllTexture()
        {
            if (m_SrcTex == null || m_DstTex == null)
                return;

            var allmat = GetAllMaterials();
            if (allmat == null)
                return;

            int count = allmat.Count;
            if (count <= 0)
                return;

            for (int i = 0; i < count; i++)
            {
                var mat = allmat[i];

                EditorUtility.DisplayProgressBar("ReplaceAllTexture", $"ReplaceAllTexture {i}/{count}", (float)(i) / count);

                var matPath = AssetDatabase.GetAssetPath(mat);
                var shader = mat.shader;
                if (shader == null)
                    continue;

                var srcTexPath = AssetDatabase.GetAssetPath(m_SrcTex);
                var dstTexPath = AssetDatabase.GetAssetPath(m_DstTex);
                var srcTexGUID = AssetDatabase.GUIDFromAssetPath(srcTexPath);
                var dstTexGUID = AssetDatabase.GUIDFromAssetPath(dstTexPath);

                var absMatPath = EditorUtils.AssetsPath2ABSPath(matPath);
                var text = File.ReadAllText(absMatPath);
                if (!text.Contains(srcTexGUID.ToString()))
                    continue;

                text = text.Replace(srcTexGUID.ToString(), dstTexGUID.ToString());

                File.WriteAllText(absMatPath, text);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.ClearProgressBar();
        }
    }
}

