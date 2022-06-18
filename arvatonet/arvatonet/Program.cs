using System;
using System.Collections.Generic;

namespace arvatonet
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.Write("TC Kimlik No girebilirsiniz:  "); //console ekranından tc kimlik no bilgisi alınıyor.
            string tc = Console.ReadLine();//Console'daki bilgiyi okudum.

            char[] rakamlarDizisi = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };//tc için kullanılacak tüm rakamları bir dizi etrafında topladım.
            Dictionary<string, Personal> dicPersonal = new Dictionary<string, Personal>(); // sistemde kayıtlı kullanıcıları  tutabilmek için ben sözlük yapısını tercih ettim. Ayrıca burada perspnel.cs adında bir sınıf oluşturdum.
            dicPersonal.Add("11111111111",new Personal { TcNumber= "11111111111", Name= "Mustafa" }); // buradaki 3 adımda da kullanıcıların tc numaraları ile birlikte isimlerini de almış oldum
            dicPersonal.Add("22222222222", new Personal { TcNumber = "22222222222", Name = "Ahmet" } );
            dicPersonal.Add("33333333333", new Personal { TcNumber = "33333333333", Name = "Can" });

            Dictionary<string, Product> dicProduct = new Dictionary<string, Product>(); // burada da ürünler için sözlük yapısını kullandım
            string appleCode = Guid.NewGuid().ToString(); // her ürün için yeni bir productCode generate ettim. ve bunları ürünlere atadım.
            string samsungCode = Guid.NewGuid().ToString();
            string xiaomiCode = Guid.NewGuid().ToString();
            dicProduct.Add(appleCode, new Product { Code=appleCode, Name = "Apple", Price = 1000, Stock=10 }); // ürünlerin özelliklerini isim, fiyat ve stok değerlerini tanımladım. Product.cs isminde bir sınıf tanımladım.
            dicProduct.Add(samsungCode, new Product { Code = samsungCode, Name = "Samsung", Price=750, Stock=100 });
            dicProduct.Add(xiaomiCode, new Product { Code = xiaomiCode, Name = "Xiaomi", Price=400, Stock=1000 });

            Dictionary<string, Order> dicOrder = new Dictionary<string, Order>();

            if (ValidationTc(tc)) // tc kimlik numarasını valide ediyorum ki 11 karakterli bir sayı alabileyim.
            {
                if (dicPersonal.ContainsKey(tc))
                {
                    Console.WriteLine(dicPersonal[tc].Name); // eğer 11 karakterliyse ve dicPersonal'daysa ismini yazdırdık.
                }
                else
                {
                    Personal personal = new Personal(); // değilse personal sözlüğüne kişiyi ekliyoruz.
                    personal.TcNumber = tc;
                    Console.Write("şimdi girdiğiniz tcye isim ekleyiniz: "); // tc kimlik no kullanıcıdan alınıyor
                    personal.Name = Console.ReadLine(); // console ekranından okundu.

                    Console.Write("şimdi girdiğiniz tcye soyisim ekleyiniz: "); //soyisim bilgisi alındı.
                    personal.Surname = Console.ReadLine();
                    Console.Write("şimdi girdiğiniz tcye gsm ekleyiniz: "); // gsm_no alındı.
                    personal.GsmNumber = Console.ReadLine();

                    dicPersonal.Add(personal.TcNumber,personal);
                    Console.WriteLine("Kayıt başarılı.");
                }
                foreach(var item in dicProduct)
                {
                    Console.WriteLine($"ÜrünKodu: {item.Key}, Ürünİsmi: {item.Value.Name}, ÜrünFiyatı: {item.Value.Price}, StoktakiÜrünSayısı: {item.Value.Stock}");
                }//Ürümşer listelendi. Ürün kodu ile birlikte isim fiyat ve stok değeri yazıldı
                Console.Write("Şimdi apple, samsung veya xiomi den bir tanesini seçebilirsiniz:  "); // seçim yapılması ve ürün kodunun girilmesi istendi.
                string productCode = Console.ReadLine();
                while (!dicProduct.ContainsKey(productCode)) //console'a girilen product bilgisi generate edilenden farklıysa Tekrar denemesini istedim.
                {
                    Console.Write("Tekrar deneyiniz  ");
                    productCode = Console.ReadLine();
                }
                Console.Write("Şimdi stok miktarını giriniz:  "); // eğer girilen ürün kodu ile generate edilen match oluyorsa stok bilgisini girmesini istedim.
                string stock = Console.ReadLine();
                int totalPiece ;
                bool isStock = int.TryParse(stock, out totalPiece);
               
                while (!isStock || dicProduct[productCode].Stock < totalPiece)  //seçilen stok miktarı gerçek stok miktarından fazlaysa tekrar stok miktarı girmesini istedik.
                {
                    Console.Write("Tekrar deneyiniz. Stok değerlerini kontrol ediniz.  ");
                    stock = Console.ReadLine();
                    isStock = int.TryParse(stock, out totalPiece);
                }
                double productPrice = dicProduct[productCode].Price; 
                Order order = new Order();
                order.Code = Guid.NewGuid().ToString();
                order.Personal = dicPersonal[tc];
                order.TotalAmount = productPrice * totalPiece;
                order.TotalPiece = totalPiece;
                order.ProductCode = productCode;
                order.ProductPrice = productPrice;

                Console.WriteLine("Sipariş başarılı bir şekilde oluşturuldu.");
                dicOrder.Add(order.Code, order);
                foreach (var item in dicOrder.Values)//sözlük yapısı kullandık ve bu sebeple for kullanamıyoruz. i değerine sahip değiliz.
                {
                    Console.WriteLine($"Sipariş kodu: {item.Code}, Tutarı: {item.TotalAmount}, Adeti: {item.TotalPiece}, Parça başına fiyatı: {item.ProductPrice}, Siparişi verenin adı : {item.Personal.Name}");
                }//$ ile + kullanımından kurtulduk.


           }
            else
            {
                Console.WriteLine("Girmis oldugunuz tc kimlik numarsi 11 karakterli olmalı, sizin girdiğiniz {0} karakterli", tc.Length);// eğer kullanıcı 11 haneden farklı değerde bir numara girdiyse hane sayısını ekrana yazdırdım.
            }
            Console.Read();

        }
        protected static bool ValidationTc(string tc) {//tc kimlik no 11 karakterli olmalı ve 0 ile başlamamalı.
            if (tc.Length == 11 && !tc.StartsWith("0"))
            {
                return true;
            }
            return false;
        }

    }

}
