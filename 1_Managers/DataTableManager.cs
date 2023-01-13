using LitJson;
using stDataTable;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataTableManager : MonoBehaviour
{
    static DataTableManager _uniqueInstance;
    public static DataTableManager _instance => _uniqueInstance;

    JsonData _jsonData;

    public Dictionary<int, stMonsterInitData> _monsterDataDic { get; private set; } = new Dictionary<int, stMonsterInitData>();
    public Dictionary<int, stItemData> _itemDataDic { get; private set; } = new Dictionary<int, stItemData>();
    public Dictionary<int, stShopData> _shopDataDic { get; private set; } = new Dictionary<int, stShopData>();
    public Dictionary<int, stSkillData> _skillDataDic { get; private set; } = new Dictionary<int, stSkillData>();

    void Awake()
    {
        if (_uniqueInstance == null)
        {
            _uniqueInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        LoadItemData();
        LoadMonsterInitData();
        LoadShopData();
        LoadSkillData();
    }
    void ParsingJsonMonsterInitData(JsonData data, Dictionary<int, stMonsterInitData> dic)
    {
        for (int i = 0; i < data.Count; i++)
        {
            int id = int.Parse(data[i][0].ToString()); //인덱스
            string name = data[i][1].ToString(); //이름
            int lv = int.Parse(data[i][2].ToString());
            float hp = float.Parse(data[i][3].ToString()); //데미지
            float dam = float.Parse(data[i][4].ToString());
            float def = float.Parse(data[i][5].ToString());
            float speed = float.Parse(data[i][6].ToString());
            float attdis = float.Parse(data[i][7].ToString());
            int ex = int.Parse(data[i][8].ToString());
            int min = int.Parse(data[i][9].ToString());
            int max = int.Parse(data[i][10].ToString());

            stMonsterInitData mondata = new stMonsterInitData(name, lv, hp, dam, def, speed, attdis, ex, min, max);
            dic.Add(id, mondata);
        }
    }
    void ParsingJsonItem(JsonData data, Dictionary<int, stItemData> itemDic)
    {
        for (int i = 0; i < data.Count; i++)
        {
            int key = int.Parse(data[i][0].ToString()); //아이템 key
            string name = data[i][1].ToString(); //이름
            int type = int.Parse(data[i][2].ToString()); //아이템 타입
            int item_kind = int.Parse(data[i][3].ToString()); //아이템 종류
            int stat = int.Parse(data[i][4].ToString()); //증가 능력치
            float value = float.Parse(data[i][5].ToString()); //증가 값
            int price = int.Parse(data[i][6].ToString()); //아이템 가격
            string tooltip = data[i][7].ToString(); //아이템 정보
            int image = int.Parse(data[i][8].ToString()); //아이템 이미지 배열 인덱스
            stItemData itemData = new stItemData(name, type, item_kind, stat, value, price, tooltip,image);
            itemDic.Add(key, itemData);

        }
    }
    void ParsingJsonShopData(JsonData data, Dictionary<int, stShopData> shopDic)
    {
        for (int n = 0; n < data.Count; n++)
        {
            int index = int.Parse(data[n][0].ToString());
            int npcID = int.Parse(data[n][1].ToString());
            int itemID = int.Parse(data[n][1].ToString());
            stShopData shopData = new stShopData(npcID, itemID);
            shopDic.Add(index, shopData);
        }
    }
    void ParsingJsonSkillData(JsonData data, Dictionary<int, stSkillData> skillDic)
    {
        for (int n = 0; n < data.Count; n++)
        {
            int index = int.Parse(data[n][0].ToString());
            string name = data[n][1].ToString();
            int lv = int.Parse(data[n][2].ToString());
            float dam = float.Parse(data[n][3].ToString());
            float mp = float.Parse(data[n][4].ToString());
            float time = float.Parse(data[n][5].ToString());
            string tip = data[n][6].ToString();
            int image = int.Parse(data[n][7].ToString());
            stSkillData skilldata = new stSkillData(name, lv, dam, mp, time, tip, image);
            skillDic.Add(index, skilldata);
        }
    }

    void LoadItemData()
    {
        string jsonstring;
        string path = Application.streamingAssetsPath + "\\ItemData.json";
        jsonstring = File.ReadAllText(path);
        _jsonData = JsonMapper.ToObject(jsonstring);
        ParsingJsonItem(_jsonData, _itemDataDic);
    }
    void LoadMonsterInitData()
    {
        string jsonstring;
        string path = Application.streamingAssetsPath + "\\MonsterInitData.json";
        jsonstring = File.ReadAllText(path);
        _jsonData = JsonMapper.ToObject(jsonstring);
        ParsingJsonMonsterInitData(_jsonData, _monsterDataDic);
    }
    void LoadShopData()
    {
        string jsonstring;
        string path = Application.streamingAssetsPath + "\\ShopData.json";
        jsonstring = File.ReadAllText(path);
        _jsonData = JsonMapper.ToObject(jsonstring);
        ParsingJsonShopData(_jsonData, _shopDataDic);
    }
    void LoadSkillData()
    {
        string jsonstring;
        string path = Application.streamingAssetsPath + "\\SkillData.json";
        jsonstring = File.ReadAllText(path);
        _jsonData = JsonMapper.ToObject(jsonstring);
        ParsingJsonSkillData(_jsonData, _skillDataDic);
    }

}
