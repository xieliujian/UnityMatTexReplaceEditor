# 材质贴图替换编辑器

![github](https://github.com/xieliujian/UnityMatTexReplaceEditor/blob/main/Video/1.png?raw=true)

## 核心代码

```cs

// 通过GUID的文本替换, 包括修改模型的可读属性都可以这种方式，可以加快Unity导入速度

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

```