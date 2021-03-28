> :warning: Dikkat. Bu çalışma, teknik borcu yüksek bir uygulamayı tasvir etmek üzere planlanmıştır. Yazılımcı adaylarının bu teknik borcu hafifletmek için gerekli pratikleri uygulaması beklenmektedir.

# Project-Denver

Denver, Brezilya'nın en büyük elektrik süpürge malzemeleri üreticisi için 2000li yılların başında yazılmış bir ERP ürünüdür. Ürünün kataloglama, satış, faturalama, yedek parça tedariki gibi modüllerinin olması ve dünya çapında 800den fazla bayii tarafından kullanılması planlanmıştır _(Güya)_.

## Ürünün Genel Özellikleri ve Kullandığı Teknolojiler

Denver, Microsoft .Net 1.1 ile geliştirilmeye başlanmış ve nihayi güncel sürümünde .Net 4.0'a geçirilmiş bir ürünü canlandırmaktadır. N-Tier mimariye _(Monolitik mimari yaklaşımın katmanlı modeli. Monolithic-Layered)_ uygun olacak şekilde tasarlanmış olup SQL Server veritabanı ile birlikte çalışmaktadır. İş kuralları çoğunlukla kod ve SQL Stored Procedure nesnelerine dağıtılmış gibi düşünülmelidir.

## Oyun Alanı

Projeyi indirdikten sonra teknik borç detayını görmek için SonarQube ile birlikte çalışmanızı öneririm. Bu sayede başarılı şekilde build olan çözümün kodsal sorunlarınları görebilir SonarQube'u öğrenebilir ve gerekli tedbirleri daha rahat alabilirsiniz. SonarQube'u kullanmak için Docker imajından yararlanılabilir. Aşağıdaki terminal komutu ile onu sistemimize ekleyebiliriz.

```bash
docker run -d --name sonarqube -e SONAR_ES_BOOTSTRAP_CHECKS_DISABLE=true -p 9000:9000 sonarqube:latest
```

Sonrasında http://localhost:9000 adresine giderek giriş yapılabilir. Varsayılan olarak imajdaki kullanıcı adı ve şifre _admin_ olarak belirlenmiştir. Bunu değiştirip ilerlenebilir. Sonarqube üstünden proje oluşturulduktan sonra tarama yaptırmak için bir Token oluşturmak gerekir. _(Sisteminizde Sonar-Scanner da olmalıdır. Bu araç zip dosyası olarak iner ve bir klasöre açılır. Sonrasında SonarScanner.MSBuild'un olduğu fiziki yol bilgisi, sistem genel path tanımınına eklenir ki herhangibir konumdan erişebilelim)_ 

Aşağıdaki terminal komutları project-denver için bir taramanın nasıl yapıldığını örneklemektedir.

```bash
SonarScanner.MSBuild.exe begin /k:"project-denver-dev" /d:sonar.host.url="http://localhost:9000" /d:sonar.login="3f5f3ede038224c9........"
MsBuild.exe /t:Rebuild
SonarScanner.MSBuild.exe end /d:sonar.login="3f5f3ede038224......"
```

Projeye kodlar eklendikçe ortaya aşağıdaki ekran görüntülerindekine benzer sonuçlar çıkacaktır.

İlk etapta her şey yeşil görünebilir.
![Assets/Sonarqube_2.png](Assets/Sonarqube_2.png)

Sonra ise aşağıdakine benzer bir seyir ortaya çıkabilir.
![Assets/Sonarqube_3.png](Assets/Sonarqube_3.png)

![Assets/Sonarqube_4.png](Assets/Sonarqube_4.png)

![Assets/Sonarqube_5.png](Assets/Sonarqube_5.png)

![Assets/Sonarqube_6.png](Assets/Sonarqube_6.png)

![Assets/Sonarqube_7.png](Assets/Sonarqube_7.png)

Ancak tabii ki bazı şeyler SonarQube tarafından da fark edilmez veya hesaplanmaz. Söz gelimi mimari bir dönüşüm söz konusu ise hangisine geçilmesi gerektiği, loglama ve monitoring stratejilerinin ne olacağı, servislerin konumlandırılması, modüler hale gelebilirlik ve benzeri konular SonarQube tarafından ele alınmazlar.