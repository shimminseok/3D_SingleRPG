
namespace stDataTable
{
    public struct stMonsterInitData
    {
        public string _name;
        public int Level;
        public float _hp;
        public float _dam;
        public float _def;
        public float _speed;
        public float _attDis;
        public int _ex;
        public int _minMoney;
        public int _maxMoney;
        public stMonsterInitData(string name, int lv, float hp, float dam, float def, float speed, float attDis, int ex, int min, int max)
        {
            _name = name;
            Level = lv;
            _hp = hp;
            _dam = dam;
            _def = def;
            _speed = speed;
            _attDis = attDis;
            _ex = ex;
            _minMoney = min;
            _maxMoney = max;
        }
    }
    public struct stItemData
    {
        public string _name;
        public int _itemType;
        public int _itemKind;
        public int _status;
        public float _value;
        public int _price;
        public string _toolTip;
        public int _itemImage;
        public stItemData(string name, int type, int kind, int stat, float val, int pri, string tip,int image)
        {
            _name = name;
            _itemType = type;
            _itemKind = kind;
            _status = stat;
            _value = val;
            _price = pri;
            _toolTip = tip;
            _itemImage = image;
        }
    }
    public struct stShopData
    {
        public int _npcID;
        public int _itemID;
        public stShopData(int nId, int iID)
        {
            _npcID = nId;
            _itemID = iID;
        }
    }
    public struct stSkillData
    {
        public string _name;
        public int _acquireLv;
        public float _dam;
        public float _spendMp;
        public float _coolTime;
        public string _tip;
        public int _imageIndex;
        public stSkillData(string name, int lv, float dam, float mp, float time, string tip, int image)
        {
            _name = name;
            _acquireLv = lv;
            _dam = dam;
            _spendMp = mp;
            _coolTime = time;
            _tip = tip;
            _imageIndex = image;
        }
    }
}
