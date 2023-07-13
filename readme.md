Backend Case:

Program bir resim indirip bu indirilen resimleri organize eden bir programdır.
- Program çalışmaya başladığında ilk olarak toplam indirilecek resim sayısını alacak. (count) - Ardından aynı anda kaç resim indirilmesine müsaade edileceği bilgisini alacak. (parallelism)
- Ardindan, program herhangi bir web sitesinden, birbirleri ile aynı olmayan rastgele resimleri toplam `count` miktarınca, en fazla aynı anda `parallelism` kadar aktif indirme yaparak klasörleyecek.
- Her inen resim, indirilenler arasındaki sırasına göre isimlendirilecek. Örneğin, 5. indirilen resim "5.png" şeklinde isme sahip olacak.
- Toplam indirme süreci (progress) canlı olarak ekrana yazılacak.
Kurallar:
- Indirilme esnasındaki veya indirilen resimler herhangi bir koleksiyonda bulunmamalıdır.
- Yukarıdakiyle bağlantılı olarak bir koleksiyonda toplamak gibi, MemoryStream gibi memory üzerinde
indirilen resim içeriği (byte array/content) tutulmamalıdır.
- Progress, konsola canlı olarak yazılmalı, eski progress durumu görünmemeli, her anda sadece tek bir
progress satırı görüntülenmeli.
- Islem indirme sürecinde CTRL+C ile iptal edilebilmeli, iptal edilirse resim çıktıları temizlenmelidir.
Bonus:
- C# event ve delegate kullanımı ile progress yazılması
- İsteğe bağlı olarak, program girişinde, kullanıcıdan input istemek yerine, beklenen bir Input.json dosyası
okunarak işleme direkt olarak başlanabilir. Örneğin:
{
 "Count": 200,
 "Parallelism": 5,
 "SavePath": "./outputs"
}
gibi bir `Input.json` dosyası ile program direkt olarak işleme başlayabilir.
Resim indirme kaynağı olarak aşağıdaki gibi bir web sitesi kullanılabilir.
Template:

 https://picsum.photos/{Width}/{Height}
Örnek:
https://picsum.photos/200/300
----- Kullanım Örneği -----
Işaretler:
> Bizim yorumumuz
$ Program konsolu ($ hariç)
$$$
Çok satırlı
program/konsol çıktısı
Örneği burada
bitiyor
$$$
... Bir süre sonra
< Girdi örneği (< 5 demek, programa 5 veriliyor ve 5 yazılana kadar program bu kullanıcı girdisini bekliyor demek)
< ⏎ (Enter tuşuna direkt basılması işareti)
----- Program Örneği -----
$ Enter the number of images to download:
< 200
$ Enter the maximum parallel download limit: <5
$ Enter the save path (default: ./outputs) <⏎
$$$
Downloading 200 images (5 parallel downloads at most)
Progress: 1/200
$$$
...

 $$$
Downloading 200 images (5 parallel downloads at most)
Progress: 40/200
$$$
