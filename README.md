# rabbitmq_net8_EFCore
Rabbit MQ

RabbitMQ ve .NET 8 ile Etkili Mesajlaşma
Son yıllarda, mikro hizmet mimarileri ve dağıtılmış sistemlerde mesajlaşma ve olay tabanlı tasarımlar ön planda. Bu makalede, RabbitMQ ve .NET 8 kullanarak profesyonel bir mesajlaşma sistemi nasıl kurulacağını, adım adım açıklayacağız. Ayrıca, Entity Framework Core (EF Core) ile veritabanı entegrasyonunu ve Hub/Subscribe (Publisher/Subscriber) mekanizmasını nasıl uygulayacağınızı öğreneceksiniz.

1. Proje Başlangıcı
Başlangıçta, bir .NET 8 projesi oluşturduk ve RabbitMQ ile temel entegrasyonu gerçekleştirdik. Proje senaryosu olarak sipariş yönetimi üzerine odaklandık.

1.1. Proje Oluşturma
Yeni bir .NET 8 Web API projesi başlattık ve temel yapılandırmalarımızı gerçekleştirdik.

1.2. RabbitMQ Bağımlılıklarını Ekledik
RabbitMQ ile etkileşim için gerekli bağımlılıkları projemize ekledik.

2. RabbitMQ Kurulumu ve Yapılandırması
RabbitMQ'yu Windows üzerinde kurarak mesajlaşma kuyruğumuzu yönettik. RabbitMQ'nun yönetim paneli üzerinden kuyrukları ve mesajları izledik.

2.1. RabbitMQ'yu İndirdik ve Kurduk
RabbitMQ'nun Windows sürümünü RabbitMQ'nun resmi web sitesinden edindik. İndirilen ZIP dosyasını uygun bir dizine çıkardık (örneğin, C:\RabbitMQ). Ayrıca, RabbitMQ'nun çalışması için gerekli olan Erlang'ı Erlang'ın resmi web sitesinden temin ettik ve kurduk.

2.2. RabbitMQ Server'ını Başlattık
RabbitMQ server'ını başlatmak için aşağıdaki adımları izledik:

RabbitMQ'nun sbin Dizine Gitmek: RabbitMQ'nun kurulu olduğu dizine giderek sbin klasörüne geçtik:

cd C:\RabbitMQ\sbin
RabbitMQ Server'ını Başlattık: RabbitMQ server'ını çalıştırmak için aşağıdaki komutu kullandık:

rabbitmq-server.bat
Arka planda çalıştırmak isterseniz, start komutunu kullanabilirsiniz:

rabbitmq-server.bat start
2.3. RabbitMQ Yönetim Panelini Etkinleştirdik
Yönetim panelinin etkinleştirilmesi için aşağıdaki komutu çalıştırdık:

rabbitmq-plugins enable rabbitmq_management
2.4. RabbitMQ Yönetim Paneline Erişim
Yönetim paneline erişim sağlamak için web tarayıcımızdan http://localhost:15672/ adresini ziyaret ettik. Varsayılan kullanıcı adı ve şifresi guest/guest olarak belirlendi.

Kullanıcı Adı: guest
Şifre: guest
3. Entity Framework Core Entegrasyonu
EF Core ile veritabanı işlemlerimizi yönettik ve siparişler ile loglar için gerekli tabloları oluşturduk.

3.1. Veritabanı Modelini Tanımladık
Siparişler ve loglar için gerekli model sınıflarını tanımladık.

3.2. DbContext Sınıfını Güncelledik
EF Core ile veritabanı bağlamını yapılandırdık ve gerekli DbContext sınıfını oluşturduk.

3.3. Migration ve Veritabanı Güncellemeleri
Veritabanı şemasını güncellemek için migration işlemlerini gerçekleştirdik.

4. Hub/Subscribe Mekanizması
Sipariş oluşturulduğunda, RabbitMQ'ya mesaj gönderilmesini ve bu mesajların veritabanına kaydedilmesini sağladık.

4.1. RabbitMQ Publisher Sınıfı
Sipariş oluşturulduğunda mesaj gönderen bir publisher sınıfı oluşturduk.

4.2. RabbitMQ Subscriber Sınıfı
Mesajları dinleyip veritabanına kaydeden bir subscriber sınıfı oluşturduk.

4.3. Publisher ve Subscriber'ı Hizmetler Olarak Konfigüre Ettik
Publisher ve subscriber hizmetlerini Scoped ve HostedService olarak kaydettik.
