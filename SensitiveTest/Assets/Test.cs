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
                text4.text = "��ƥ��������" + loopCount * words.Length;
            }
        });

        btn_string.onClick.AddListener(() => {
            count1 ++;
            text_string_count.text = "����ƥ�������" + count1;
            sw.Restart();
            for (int i = 0; i < loopCount; i++)
            {
                Check_Contains(words);
            }
            sw.Stop();
            text_string_time.text = "ƥ��ʱ�䣺" + sw.ElapsedMilliseconds;
        });

        btn_trie.onClick.AddListener(() =>
        {
            count2++;
            text_trie_count.text = "����ƥ�������" + count2;
            sw.Restart();
            for (int i = 0; i < loopCount; i++)
            {
                Check_TrieTree(words);
            }
            sw.Stop();
            text_trie_time.text = "ƥ��ʱ�䣺" + sw.ElapsedMilliseconds;
        });
        btn_ac.onClick.AddListener(() =>
        {
            count3++;
            text_ac_count.text = "����ƥ�������" + count3;
            sw.Restart();
            for (int i = 0; i < loopCount; i++)
            {
                Check_AC(words);
            }
            sw.Stop();
            text_ac_time.text = "ƥ��ʱ�䣺" + sw.ElapsedMilliseconds;
        });

        btn_fenci.onClick.AddListener(() => {
            sw.Restart();
            output.text = TestIKAnalyzer(input2.text);
            sw.Stop();
            iktime.text = "�ִ�ʱ�䣺" + sw.ElapsedMilliseconds;
        });
    }


    static string words = "����+�������ҹ������ܷ�����ʩ��40���ꡣ12��19�գ�ϰ��ƽ����Ƿ�����Ҫ����������Ʒ���¡���д��ʱ���й��ܷ�ʵ����ƪ�¡������������ܷ�����ʩ��40���꡷�����»ع���40�����ҹ��ܷ�ʩ��������ܽ�����ʱ��ʮ���ҹ��ܷ��ƶȽ�����ܷ�ʵʩ�ලȡ�õ��ش��Ч�Լ����ʾ��飬��Ϊ��ʱ���й��ܷ�ʵ��ָ��·����������������+�����������������Ҫ�㣬���϶��ߡ�" +
                        "��ʱ��ȫ��᳹ʵʩ�ܷ������ʾ���" +
                        "��ʱ��ʮ���ҹ��ܷ��ƶȽ�����ܷ�ʵʩ�ලȡ���ش��Ч��ȫ��ȫ����ܷ���ʶ����������������巨�ν���ɹ���˶�����ǵ��ܽ�������ʷ���飬ȫ��᳹ʵʩ�ܷ��������ƽ��ܷ����ۺ��ܷ�ʵ�����£�������������ʾ��飬��˶��ҹ��ܷ��ƶȽ���Ĺ�������ʶ��ϰ��ƽ������������о͹�������ʶ�����ܽ᣺" +
                        "һ�Ǳ������й��������쵼��" +
                        "���Ǳ��������񵱼�������" +
                        "���Ǳ����������ι�������ִ�� ��" +
                        "���Ǳ������ܷ���ϰ���۹��Ҹ�������λ��" +
                        "���Ǳ������ܷ�ʵʩ��ල�ƶȻ����滯��" +
                        "���Ǳ�����ά���ܷ�Ȩ�������ϡ�" +
                        "���Ǳ�������ʱ������ƺͷ�չ�ܷ���" +
                        "��д��ʱ���й��ܷ�ʵ�����µ���ƪ��" +
                        "���Ķ�ʮ�����ʱ�������̵��͹�����ҵ��չ����ȫ�沿��ǿ��Ҫ���÷����ܷ����ι������е���Ҫ���ã����÷��ӷ��ι̸�������Ԥ�ڡ�����Զ�ı������ã��ڷ��ι����ȫ�潨����������ִ������ҡ�ϰ��ƽ�������������ǿ��������Ҫ�᳹��ʵ���Ķ�ʮ���񣬼ᶨ�������й���ɫ������巨�ε�·����ǿ�ܷ��Ծ�����ǿ�ܷ�ʵʩ�������ܷ�ʹ������д��ʱ���й��ܷ�ʵ����ƪ�¡�" +
                        "��һ����ֺͼ�ǿ�����ܷ�������ȫ���쵼�����÷����ҹ��ܷ��ƶȵ��������ƺ���Ҫ���á�" +
                        "�ڶ������ܷ�ʵʩ�ᴩ���ι�����������ȫ���̣�������ߵ������ι�������ִ����������" +
                        "�������ӿ��������ܷ�Ϊ���ĵ��й���ɫ�����С��ά���巨����ϵ��������ǿ���ɹ淶��ϵ��ȫ���ԡ�ϵͳ�ԡ�Э���ԡ�" +
                        "���ģ���ȫ��֤�ܷ�ȫ��ʵʩ���ƶ���ϵ����������ܷ�ʵʩ�ͼලˮƽ��" +
                        "���壬��ǿ�ܷ������о����������������������й��ܷ����ۺ�ʵ����˵������Ӱ������";



    private static string[] thesaurus;

    public void Initialize()
    {
        var ta = Resources.Load<TextAsset>("SNH");
        //UnityEngine.Debug.Log(ta.text);
        OnDownLoadFinish(ta.text);
    }

    /// <summary>
    /// �Ƿ�������д�
    /// </summary>
    /// <param name="content">ƥ������</param>
    /// <returns></returns>
    public  bool Check(string content)
    {
        return Check_TrieTree(content);
    }


    private  void OnDownLoadFinish(string data)
    {
        //UnityEngine.Debug.Log("�����дʿ⡿�������:" + data);
        thesaurus = data.Split('\n');
        //UnityEngine.Debug.Log("�����дʿ⡿�ʿ��ܵ�������" + thesaurus.Length);
        text1.text = "�ֿ�����Ŀ����" + thesaurus.Length;
        text2.text = "ƥ������������" + words.Length;

        
        sw.Start();
        CreateTrieTree();        
        sw.Stop();
        text_trie_buildtime.text = "TrieTree����ʱ�䣺" + sw.ElapsedMilliseconds;
        //UnityEngine.Debug.Log("�����дʿ⡿ACTree����ʱ��" + sw.ElapsedMilliseconds);
        sw.Restart();
        CreateACTree();        
        sw.Stop();
        text_ac_buildtime.text = "AC�Զ���TrieTre����ʱ�䣺" + sw.ElapsedMilliseconds;
        sw.Restart();
        CreateACTreeFailNode();
        sw.Stop();
        text_ac_failtime.text = "ac�Զ���ʧ��ָ�빹��ʱ�䣺" + sw.ElapsedMilliseconds;
        //UnityEngine.Debug.Log("�����дʿ⡿ACTree ʧ��ָ�빹��ʱ��" + sw.ElapsedMilliseconds);
       
        //UnityEngine.Debug.Log("�����дʿ⡿ƥ��ʱ�䣺" + sw.ElapsedMilliseconds);
    }

    #region String.Contains
    private static bool Check_Contains(string content)
    {
        for (int i = 0; i < thesaurus.Length; i++)
        {
            if (content.Contains(thesaurus[i]))
            {
                //UnityEngine.Debug.Log("�����дʿ⡿ƥ�䵽���дʣ�" + thesaurus[i]);
                return false;
            }
        }
        //UnityEngine.Debug.Log("�����дʿ⡿ƥ�����,δ�������д�");
        return true;
    }
    #endregion

    #region TrieTree
    private static TrieNode rootNode;
    private static void CreateTrieTree()
    {
        //UnityEngine.Debug.Log("�����дʿ⡿��ʼ����trie��");
        rootNode = new TrieNode();
        for (int i = 0; i < thesaurus.Length; i++)
        {
            char[] charArray = thesaurus[i].ToCharArray();
            rootNode.AddSubNode(charArray);
        }
        //UnityEngine.Debug.Log("�����дʿ⡿����trie�����");
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
                //��һ�����д�
                sb.Append(tmpChar);
                beginIndex = ++posIndex;
                //UnityEngine.Debug.Log("�����дʿ⡿ƥ�䵽���дʣ�" + sb.ToString());
            }
            else
            {
                //���������һ���ַ�
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

    #region AC�Զ���

    private static ACNode acRootNode;

    private static void CreateACTree()
    {
        //UnityEngine.Debug.Log("�����дʿ⡿��ʼ����AC��");
        acRootNode = new ACNode();
        for (int i = 0; i < thesaurus.Length; i++)
        {
            char[] charArray = thesaurus[i].ToCharArray();
            acRootNode.AddSubNode(charArray);
        }
        //UnityEngine.Debug.Log("�����дʿ⡿����AC�����");
    }

    private static void CreateACTreeFailNode()
    {
        //UnityEngine.Debug.Log("�����дʿ⡿��ʼ����AC�Զ�����ʧ��ָ��");
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
        //UnityEngine.Debug.Log("�����дʿ⡿����AC�Զ�����ʧ��ָ�����");
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
                    //UnityEngine.Debug.Log("�����дʿ⡿ƥ�䵽���дʣ�" + tmpNode.ToString());
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

    #region �ִ���
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