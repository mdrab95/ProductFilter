namespace ProductFilter
{
    class Worker
    {
        private int _Id;
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        private int _personId;
        public int PersonId
        {
            get { return _personId; }
            set { _personId = value; }
        }

        private string _personData;
        public string PersonData
        {
            get { return _personData; }
            set { _personData = value; }
        }

        private string _department;
        public string Department
        {
            get { return _department; }
            set { _department = value; }
        }

        private string _item;
        public string Item
        {
            get { return _item; }
            set { _item = value; }
        }

        private string _itemCount;
        public string ItemCount
        {
            get { return _itemCount; }
            set { _itemCount = value; }
        }

        private string _itemProperty;
        public string ItemProperty
        {
            get { return _itemProperty; }
            set { _itemProperty = value; }
        }

        private string _year;
        public string Year
        {
            get { return _year; }
            set { _year = value; }
        }
    }
}
