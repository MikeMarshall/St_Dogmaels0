using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using SQLite;

namespace St_Dogmaels
{
    // id	longitude	latitude	housename	housenumber	street	city	postcode	name	name_cy	is_in	place	alt_name	open_hours	phone	shop	url


    [Table("Buildings")]
    public class Building : INotifyPropertyChanged
    {
        private int _id;
        [PrimaryKey]
        public int Id
        {
            get
            {
                return _id;
            }

            set
            {
                this._id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        private double _longitude;
        [NotNull]


        public double Longitude
        {
            get
            {
                return _longitude;
            }
            set
            {
                this._longitude = value;
                OnPropertyChanged(nameof(Longitude));
            }
        }

        private double _latitude;
        [NotNull]


        public double Latitude
        {
            get
            {
                return _latitude;
            }
            set
            {
                this._latitude = value;
                OnPropertyChanged(nameof(Latitude));
            }
        }


        private string _housename;
        [MaxLength(50)]
        public string Housename
        {
            get
            {
                return _housename;
            }
            set
            {
                this._housename = value;
                OnPropertyChanged(nameof(Housename));
            }
        }


        private string _housenumber;
        [MaxLength(50)]
        public string Housenumber
        {
            get
            {
                return _housenumber;
            }
            set
            {
                this._housenumber = value;
                OnPropertyChanged(nameof(Housenumber));
            }
        }


        private string _street;
        [MaxLength(50)]
        public string Street
        {
            get
            {
                return _street;
            }
            set
            {
                this._street = value;
                OnPropertyChanged(nameof(Street));
            }
        }


        private string _city;
        [MaxLength(50)]
        public string City
        {
            get
            {
                return _city;
            }
            set
            {
                this._city = value;
                OnPropertyChanged(nameof(City));
            }
        }


        private string _postcode;
        [MaxLength(50)]
        public string Postcode
        {
            get
            {
                return _postcode;
            }
            set
            {
                this._postcode = value;
                OnPropertyChanged(nameof(Postcode));
            }
        }


        private string _name;
        [MaxLength(50)]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                this._name = value;
                OnPropertyChanged(nameof(Name));
            }
        }


        private string _name_cy;
        [MaxLength(50)]
        public string Name_cy
        {
            get
            {
                return _name_cy;
            }
            set
            {
                this._name_cy = value;
                OnPropertyChanged(nameof(Name_cy));
            }
        }


        private string _is_in;
        [MaxLength(50)]
        public string Is_in
        {
            get
            {
                return _is_in;
            }
            set
            {
                this._is_in = value;
                OnPropertyChanged(nameof(Is_in));
            }
        }


        private string _place;
        [MaxLength(50)]
        public string Place
        {
            get
            {
                return _place;
            }
            set
            {
                this._place = value;
                OnPropertyChanged(nameof(Place));
            }
        }


        private string _alt_name;
        [MaxLength(50)]
        public string Alt_name
        {
            get
            {
                return _alt_name;
            }
            set
            {
                this._alt_name = value;
                OnPropertyChanged(nameof(Alt_name));
            }
        }


        private string _open_hours;
        [MaxLength(50)]
        public string Open_hours
        {
            get
            {
                return _open_hours;
            }
            set
            {
                this._open_hours = value;
                OnPropertyChanged(nameof(Open_hours));
            }
        }


        private string _phone;
        [MaxLength(50)]
        public string Phone
        {
            get
            {
                return _phone;
            }
            set
            {
                this._phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }


        private string _shop;
        [MaxLength(50)]
        public string Shop
        {
            get
            {
                return _shop;
            }
            set
            {
                this._shop = value;
                OnPropertyChanged(nameof(Shop));
            }
        }


        private string _url;
        [MaxLength(50)]
        public string Url
        {
            get
            {
                return _url;
            }
            set
            {
                this._url = value;
                OnPropertyChanged(nameof(Url));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



    }
}
