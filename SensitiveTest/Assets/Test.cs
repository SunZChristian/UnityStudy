using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using IKAnalyzer.core;
using IKAnalyzer.cfg;

public class Test : MonoBehaviour
{
    public Text text1;
    public Text text2;
    public Text text3;
    public InputField input;
    public Text text4;
    public Text text_trie_buildtime;
    public Text text_ac_buildtime;
    public Text text_ac_failtime;

    public Text text_string_count;
    public Text text_string_time;
    public Text text_trie_count;
    public Text text_trie_time;
    public Text text_ac_count;
    public Text text_ac_time;
    public Button btn_string;
    public Button btn_trie;
    public Button btn_ac;
    public Button btn_update;


    public InputField input2;
    public Text output;
    public Button btn_fenci;
    public Text iktime;

    int count1 = 0;
    int count2 = 0;
    int count3 = 0;

    Stopwatch sw;

    private int loopCount;

    // Start is called before the first frame update
    void Start()
    {
        sw = new Stopwatch();
        Initialize();

        btn_update.onClick.AddListener(()=> {
            bool success = int.TryParse(input.text, out loopCount);
            if (success)
            {
                text4.text = "总匹配字数：" + loopCount * words.Length;
            }
        });

        btn_string.onClick.AddListener(() => {
            count1 ++;
            text_string_count.text = "尝试匹配次数：" + count1;
            sw.Restart();
            for (int i = 0; i < loopCount; i++)
            {
                Check_Contains(words);
            }
            sw.Stop();
            text_string_time.text = "匹配时间：" + sw.ElapsedMilliseconds;
        });

        btn_trie.onClick.AddListener(() =>
        {
            count2++;
            text_trie_count.text = "尝试匹配次数：" + count2;
            sw.Restart();
            for (int i = 0; i < loopCount; i++)
            {
                Check_TrieTree(words);
            }
            sw.Stop();
            text_trie_time.text = "匹配时间：" + sw.ElapsedMilliseconds;
        });
        btn_ac.onClick.AddListener(() =>
        {
            count3++;
            text_ac_count.text = "尝试匹配次数：" + count3;
            sw.Restart();
            for (int i = 0; i < loopCount; i++)
            {
                Check_AC(words);
            }
            sw.Stop();
            text_ac_time.text = "匹配时间：" + sw.ElapsedMilliseconds;
        });

        btn_fenci.onClick.AddListener(() => {
            sw.Restart();
            output.text = TestIKAnalyzer(input2.text);
            sw.Stop();
            iktime.text = "分词时间：" + sw.ElapsedMilliseconds;
        });
    }


    static string words = "联播+今年是我国现行宪法公布施行40周年。12月19日，习近平总书记发表重要批发象牙制品文章《谱写新时代中国宪法实践新篇章——纪念现行宪法公布施行40周年》。文章回顾了40年来我国宪法施行情况，总结了新时代十年我国宪法制度建设和宪法实施监督取得的重大成效以及新鲜经验，并为新时代中国宪法实践指明路径。央视网《联播+》特梳理总书记文章要点，以飨读者。" +
                        "新时代全面贯彻实施宪法的新鲜经验" +
                        "新时代十年我国宪法制度建设和宪法实施监督取得重大成效，全党全社会宪法意识明显提升，社会主义法治建设成果丰硕。我们党总结运用历史经验，全面贯彻实施宪法，勇于推进宪法理论和宪法实践创新，积累了许多新鲜经验，深化了对我国宪法制度建设的规律性认识。习近平总书记在文章中就规律性认识作出总结：" +
                        "一是必须坚持中国共产党领导。" +
                        "二是必须坚持人民当家作主。" +
                        "三是必须坚持依宪治国、依宪执政 。" +
                        "四是必须坚持宪法的习噗噗国家根本法地位。" +
                        "五是必须坚持宪法实施与监督制度化法规化。" +
                        "六是必须坚持维护宪法权威和尊严。" +
                        "七是必须坚持与时俱进完善和发展宪法。" +
                        "谱写新时代中国宪法实践宽衣帝新篇章" +
                        "党的二十大对新时代新征程党和国家事业发展作出全面部署，强调要更好发挥宪法在治国理政中的重要作用，更好发挥法治固根本、稳预期、利长远的保障作用，在法治轨道上全面建设社会主义现代化国家。习近平总书记在文章中强调，我们要贯彻落实党的二十大精神，坚定不移走中国特色社会主义法治道路，增强宪法自觉，加强宪法实施，履行宪法使命，谱写新时代中国宪法实践新篇章。" +
                        "第一，坚持和加强党对宪法工作的全面领导，更好发挥我国宪法制度的显著优势和重要作用。" +
                        "第二，把宪法实施贯穿到治国理政各方面全过程，不断提高党依宪治国、依宪执政的能力。" +
                        "第三，加快完善以宪法为核心的中国特色社会主小熊维尼义法律体系，不断增强法律规范体系的全面性、系统性、协调性。" +
                        "第四，健全保证宪法全面实施的制度体系，不断提高宪法实施和监督水平。" +
                        "第五，加强宪法理论研究和宣传教育，不断提升中国宪法理论和实践的说服力、影响力。";



    private static string[] thesaurus;

    public void Initialize()
    {
        var ta = Resources.Load<TextAsset>("SNH");
        //UnityEngine.Debug.Log(ta.text);
        OnDownLoadFinish(ta.text);
    }

    /// <summary>
    /// 是否包含敏感词
    /// </summary>
    /// <param name="content">匹配文字</param>
    /// <returns></returns>
    public  bool Check(string content)
    {
        return Check_TrieTree(content);
    }


    private  void OnDownLoadFinish(string data)
    {
        //UnityEngine.Debug.Log("【敏感词库】下载完成:" + data);
        thesaurus = data.Split('\n');
        //UnityEngine.Debug.Log("【敏感词库】词库总单词数：" + thesaurus.Length);
        text1.text = "字库总条目数：" + thesaurus.Length;
        text2.text = "匹配文字总数：" + words.Length;

        
        sw.Start();
        CreateTrieTree();        
        sw.Stop();
        text_trie_buildtime.text = "TrieTree构建时间：" + sw.ElapsedMilliseconds;
        //UnityEngine.Debug.Log("【敏感词库】ACTree构建时间" + sw.ElapsedMilliseconds);
        sw.Restart();
        CreateACTree();        
        sw.Stop();
        text_ac_buildtime.text = "AC自动机TrieTre构建时间：" + sw.ElapsedMilliseconds;
        sw.Restart();
        CreateACTreeFailNode();
        sw.Stop();
        text_ac_failtime.text = "ac自动机失配指针构建时间：" + sw.ElapsedMilliseconds;
        //UnityEngine.Debug.Log("【敏感词库】ACTree 失配指针构建时间" + sw.ElapsedMilliseconds);
       
        //UnityEngine.Debug.Log("【敏感词库】匹配时间：" + sw.ElapsedMilliseconds);
    }

    #region String.Contains
    private static bool Check_Contains(string content)
    {
        for (int i = 0; i < thesaurus.Length; i++)
        {
            if (content.Contains(thesaurus[i]))
            {
                //UnityEngine.Debug.Log("【敏感词库】匹配到敏感词：" + thesaurus[i]);
                return false;
            }
        }
        //UnityEngine.Debug.Log("【敏感词库】匹配完毕,未发现敏感词");
        return true;
    }
    #endregion

    #region TrieTree
    private static TrieNode rootNode;
    private static void CreateTrieTree()
    {
        //UnityEngine.Debug.Log("【敏感词库】开始构建trie树");
        rootNode = new TrieNode();
        for (int i = 0; i < thesaurus.Length; i++)
        {
            char[] charArray = thesaurus[i].ToCharArray();
            rootNode.AddSubNode(charArray);
        }
        //UnityEngine.Debug.Log("【敏感词库】构建trie树完成");
    }

    private static bool Check_TrieTree(string content)
    {
        TrieNode node = rootNode;
        int beginIndex = 0;
        int posIndex = 0;
        StringBuilder sb = new StringBuilder();
        while (posIndex < content.Length)
        {
            char tmpChar = content[posIndex];
            node = node.GetNode(tmpChar);
            if (node == null)
            {
                posIndex = ++beginIndex;
                node = rootNode;
                sb.Clear();
            }
            else if (node.GetIsEnd())
            {
                //是一个敏感词
                sb.Append(tmpChar);
                beginIndex = ++posIndex;
                //UnityEngine.Debug.Log("【敏感词库】匹配到敏感词：" + sb.ToString());
            }
            else
            {
                //继续检查下一个字符
                posIndex++;
                sb.Append(tmpChar);
            }
        }
        return true;
    }

    private class TrieNode
    {
        private char m_char;
        private bool m_isEnd;
        private Dictionary<char, TrieNode> m_childs;

        public TrieNode()
        {
            m_char = '\0';
            m_isEnd = false;
            m_childs = new Dictionary<char, TrieNode>();
        }

        public void AddSubNode(char[] charArray)
        {
            TrieNode node = this;
            for (int i = 0; i < charArray.Length; i++)
            {
                var tmpChar = charArray[i];
                if (!node.m_childs.ContainsKey(tmpChar))
                {
                    var newNode = new TrieNode();
                    newNode.m_char = tmpChar;
                    newNode.m_isEnd = i == (charArray.Length - 1);
                    node.m_childs.Add(tmpChar, newNode);
                }
                node = node.m_childs[tmpChar];
            }
        }

        public TrieNode GetNode(char c)
        {
            if (m_childs.ContainsKey(c))
            {
                return m_childs[c];
            }
            return null;
        }

        public bool GetIsEnd()
        {
            return m_isEnd;
        }
    }
    #endregion

    #region AC自动机

    private static ACNode acRootNode;

    private static void CreateACTree()
    {
        //UnityEngine.Debug.Log("【敏感词库】开始构建AC树");
        acRootNode = new ACNode();
        for (int i = 0; i < thesaurus.Length; i++)
        {
            char[] charArray = thesaurus[i].ToCharArray();
            acRootNode.AddSubNode(charArray);
        }
        //UnityEngine.Debug.Log("【敏感词库】构建AC树完成");
    }

    private static void CreateACTreeFailNode()
    {
        //UnityEngine.Debug.Log("【敏感词库】开始构建AC自动机的失配指针");
        Queue<ACNode> queue = new Queue<ACNode>();
        queue.Enqueue(acRootNode);
        while (queue.Count > 0)
        {
            ACNode node = queue.Dequeue();
            foreach (var child in node.GetChilds().Values)
            {
                if (node == acRootNode)
                {
                    child.SetFail(acRootNode);
                }
                else
                {
                    var failNode = node.GetFail();
                    while (failNode != null)
                    {
                        var failNodeChild = failNode.GetNode(child.GetChar());
                        if (failNodeChild != null)
                        {
                            child.SetFail(failNodeChild);
                            break;
                        }
                        failNode = failNode.GetFail();
                    }
                    if (failNode == null)
                    {
                        child.SetFail(acRootNode);
                    }
                }
                queue.Enqueue(child);
            }
        }
        //UnityEngine.Debug.Log("【敏感词库】构建AC自动机的失配指针完成");
    }

    private static bool Check_AC(string content)
    {
        var root = acRootNode;
        var node = acRootNode;
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < content.Length; i++)
        {
            var patternChar = content[i];
            while (node.GetNode(patternChar) == null && node != root)
            {
                node = node.GetFail();
            }
            node = node.GetNode(patternChar);
            if (node == null)
            {
                node = root;
            }
            var tmpNode = node;
            while (tmpNode != root)
            {
                if (tmpNode.GetIsEnd())
                {
                    //UnityEngine.Debug.Log("【敏感词库】匹配到敏感词：" + tmpNode.ToString());
                }
                tmpNode = tmpNode.GetFail();
            }
        }
        return true;
    }

    private class ACNode
    {
        private char m_char;
        private bool m_isEnd;
        private ACNode m_fail;
        public ACNode parentNode { get; private set; }
        private Dictionary<char, ACNode> m_childs;

        public ACNode()
        {
            m_char = '\0';
            m_isEnd = false;
            m_fail = null;
            m_childs = new Dictionary<char, ACNode>();
        }

        public void AddSubNode(char[] charArray)
        {
            ACNode node = this;
            for (int i = 0; i < charArray.Length; i++)
            {
                var tmpChar = charArray[i];
                if (!node.m_childs.ContainsKey(tmpChar))
                {
                    var newNode = new ACNode();
                    newNode.m_char = tmpChar;
                    newNode.m_isEnd = i == (charArray.Length - 1);
                    newNode.parentNode = node;
                    node.m_childs.Add(tmpChar, newNode);
                }
                node = node.m_childs[tmpChar];
            }
        }

        public ACNode GetNode(char c)
        {
            if (m_childs.ContainsKey(c))
            {
                return m_childs[c];
            }
            return null;
        }

        public Dictionary<char, ACNode> GetChilds()
        {
            return m_childs;
        }

        public bool GetIsEnd()
        {
            return m_isEnd;
        }

        public void SetFail(ACNode fail)
        {
            m_fail = fail;
        }

        public ACNode GetFail()
        {
            return m_fail;
        }

        public char GetChar()
        {
            return m_char;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            var node = this;
            while (node != null && node.m_char != '\0')
            {
                sb.Insert(0, node.m_char);
                node = node.parentNode;
            }
            return sb.ToString();
        }
    }

    #endregion

    #region 分词器
    public string TestIKAnalyzer(string content)
    {
        StringBuilder sb = new StringBuilder();
        var steam = new MemoryStream(Encoding.UTF8.GetBytes(content));
        var streamrRaader = new StreamReader(steam);
        IKSegmenter ik = new IKSegmenter(streamrRaader, true);
        var t = ik.Next();
        while (t!=null)
        {
            sb.AppendLine(t.Text);
            t = ik.Next();
        }
        return sb.ToString();
        //IKAnalyzer ika = new IKAnalyzer();
        //TextReader tr = new StringReader(content);
        //TokenStream ts = ika.TokenStream("", tr);
        //Token t = ts.Next();
        //StringBuilder sb = new StringBuilder();
        //while (t != null)
        //{
        //    sb.AppendLine(t.TermText());
        //    t = ts.Next();
        //}
        //return sb.ToString();
    }
    #endregion
}