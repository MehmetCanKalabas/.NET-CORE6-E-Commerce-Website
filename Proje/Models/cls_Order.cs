using Proje.Models.MVVM;

namespace Proje.Models
{
    public class cls_Order
    {
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public string MyCart { get; set; }
        public decimal UnitPrice { get; set; }
        public string ProductName { get; set; }
        public int Kdv { get; set; }
        public string PhotoPath { get; set; }

        iakademi45Context context = new iakademi45Context();

        //sepete ekle
        //10=1&
        //20=2&&
        //18=1
        public bool AddToMyCart(string id)
        {
            bool exists = false;

            if (MyCart == "")
            {
                MyCart = id + "=1";
            }
            else
            {
                string[] MyCartArray = MyCart.Split('&');
                for (int i = 0; i < MyCartArray.Length; i++)
                {
                    //for ilk dönüşünde içinde ki kayıt 10=1
                    //for ikinci dönüşünde içinde ki kayıt 20=2
                    //for üçüncü dönüşünde içinde ki kayıt 18=1
                    string[] MyCartArrayLoop = MyCartArray[i].Split('=');
                    if (MyCartArrayLoop[0] == id)
                    {
                        //Bu ürün sepete önceden eklenmiş
                        //MyCartArrayLoop[1] += 1; //Aynı üründen eklemek istersek
                        exists = true;
                    }
                }
                if (exists == false)
                {
                    MyCart = MyCart + "&" + id.ToString() + "=1";
                }
            }
            return exists;
        }

        //sepet sayfasında ürünün bilgilerini getir
        public List<cls_Order> SelectMyCart()
        {
            //10=1&
            //20=1&
            //30=4&
            //40=2
            List<cls_Order> list = new List<cls_Order>();
            string[] MyCartArray = MyCart.Split('&');
            if (MyCartArray[0] != "")
            {
                for (int i = 0; i < MyCartArray.Length; i++)
                {
                    string[] MyCartArrayLoop = MyCartArray[i].Split('=');
                    int MyCartID = Convert.ToInt32(MyCartArrayLoop[0]);

                    Product? prd = context.Products.FirstOrDefault(p => p.ProductID == MyCartID);

                    // veritabanındaki verileri propertylere yazdırdım
                    cls_Order ord = new cls_Order();
                    ord.ProductID = prd.ProductID;
                    ord.Quantity = Convert.ToInt32(MyCartArrayLoop[1]);
                    ord.UnitPrice = prd.UnitPrice;
                    ord.ProductName = prd.ProductName;
                    ord.Kdv = prd.Kdv;
                    ord.PhotoPath = prd.PhotoPath;
                    list.Add(ord);
                }
            }
            return list;
        }

        //10=1
        //20=1
        //30=4
        //40=2
        public void DeleteFromMyCart(string id)
        {           
            string[] MyCartArray = MyCart.Split('&');
            string NewMyCart = "";
            int count = 1;

            for (int i = 0; i < MyCartArray.Length; i++)
            {
                //ProductID ile adet ayrıldı
                string[] MyCartArrayLoop = MyCartArray[i].Split('=');
                //for her döndüğünde dizinin sıfırıncı alanındaki değeri(10,20,30,40) MyCardID'ye atadım
                string MyCartID = MyCartArrayLoop[0];

                if (MyCartID != id)
                {
                    //Sepetten silinmeyecek olanlar buraya girecek
                    if (count == 1)
                    {
                        NewMyCart = MyCartArrayLoop[0] + "=" + MyCartArrayLoop[1];
                        count++;
                    }
                    else
                    {
                        NewMyCart += "&" + MyCartArrayLoop[0] + "=" + MyCartArrayLoop[1];
                    }
                }
                //else
                //{                   
                    //Buraya girerse bu silinecek olan üründür.NewMyCartArray içine eklenmeyecek
                    //Sepetten silinecek olan buraya girecek
                //}
            }
            MyCart = NewMyCart;
        }

    }
}
