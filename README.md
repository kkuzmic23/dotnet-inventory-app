# Projekt 2

## Ispravak pogrešaka i unaprijeđenje programskog koda

### kkuzmic23

Prije pisanja testova bilo je potrebno refaktorirati WPFLayer i BusinessLogicLayer. Aplikacija nije bila najbolje izrađena, primjer loše prakse koja je korištena je korištenje poslovne logike u frontend-u.
<img width="859" height="851" alt="validation-product" src="https://github.com/user-attachments/assets/d5012a89-0659-41a0-912c-2b6ace8df24a" />
Većina logike u pitanju je validacija stvorenih objekata i filtriranje rezultata pretraživanja, potrebno je i za to napisati testove. Na slici je vidljiv primjer novih metoda, one sadrže logiku koja se prije nalazila u WPFLayer prozorima za Proizvode.


### tlevanic23

## Jedinično testiranje

### kkuzmic23

Prvi korak je stvaranje xUnit projekta. Dodao sam dependency na BLL i EntityLayer. Stvorio sam sljedeće datoteke:
<img width="196" height="163" alt="test-datoteke" src="https://github.com/user-attachments/assets/09f18d77-6d3a-48a5-bf3d-76ebb1aa6ae1" />
Svaka testna datoteka pokreće i testira servise. Npr. SupplierTests testira SupplierService. Jedina iznimka je UserTests, koja testira i PasswordHasher.cs datoteku.

Sljedeća slika prikazuje jednu metodu koja se testira:
<img width="788" height="360" alt="metoda-bez-repozitorija" src="https://github.com/user-attachments/assets/f1685f8a-3809-48ee-99f9-289005bd308d" />
Ovo je prvi tip metode. Ona ne poziva repozitorij te ne zahtjeva implementaiju dependency injection-a (niti FakeItEasy). Zbog toga, pisanje testova za ovakve metode znatno je brže i jednostavije.

Sljedeća slika prikazuje test takve neovisne metode:
<img width="499" height="309" alt="fact-test-bez-fakeiteasy" src="https://github.com/user-attachments/assets/70794cb8-cfa3-4549-9e62-c1e717a8e941" />
Ovaj test provjerava metodu ValidateProduct() koja provjerava ispravnost stvorenog proizvoda. Konkretno, metoda rukuje validnim objektom proizvoda te od servisa očekuje pozitivni rezultat.

Sljedeća slika prikazuje test koji je napisan pomoću [Theory]:
<img width="788" height="435" alt="theory-bez-fakeiteasy" src="https://github.com/user-attachments/assets/9db53f35-ef4c-42c9-b646-cf810f2fa039" />
Rsazlika u [Theory] i [Fact] je to da [Fact] provjerava samo jedan slučaj, dok [Theory] u jednom kodu provjerava više slučajeva. Svaki [InlineData()] redak predstavlja jedan slučaj. Korišenje [Theory] je prikladno kad testiramo nedostajuće ili neispravne unose jer možemo napraviti kombinaciju svih mogućnosti.

Problem kod unit testova je što neke metode ipak pozivaju repozitorij:
<img width="425" height="215" alt="metoda-sa-repozitorijem" src="https://github.com/user-attachments/assets/1e0bc97a-25e6-4a6e-a651-2c86020ac6fc" />
Ova metoda servisa je vrlo jednostavna, no ovisna je o ProductRepository-ju. Kako bi ju mogli testirati, potrebno je implementirati dependency-injection u servisnoj klasi. Prvi korak toga je stvaranje sučelja koje sadrži sve potrebne metode:
<img width="478" height="246" alt="i-product-crud-repository" src="https://github.com/user-attachments/assets/ea258c5e-7ac7-4461-a93d-58ae097edf0f" />
Sada u servisu možemo staviti atribut sučelja i konstruktor koji inicijalizira atribut:
<img width="555" height="265" alt="novi-dependency-injection" src="https://github.com/user-attachments/assets/0ccb7fbc-fbc1-425f-ab01-e6c1b1a9c456" />
Važno je istaknuti korištenje constructor-chaining-a. Dobit ove izvedbe je da u WPF-u netreba promjeniti stvaranje servisa. Efekt ovog konstruktor overloading-a je da default konstruktor bez argumenta inicijalizira stvarni servis. U sljedećoj slici, u testu koristimo konstruktor sa jednim argumentom:
<img width="708" height="469" alt="fact-test-sa-fakeiteasy" src="https://github.com/user-attachments/assets/8503e4a7-8c43-4f49-90fc-6b5d2a250c74" />
Sada konstruktor overloading odabire konstuktor koji ima argument te koji stvara lažni servis pomoću FakeItEasy. 

Rezultat izvođenja unit testova:
<img width="510" height="259" alt="rezultati-unit-testova" src="https://github.com/user-attachments/assets/e636134c-2ddf-4e25-9068-27b9f477adca" />


### tlevanic23

## Integracijsko testiranje

### kkuzmic23

### tlevanic23

## Uspostavljanje CI/CD cjevovoda

### kkuzmic23

### tlevanic23

## Razvoj vođen testiranjem (TDD)

### kkuzmic23

### tlevanic23

## Recenzija programskog koda

### kkuzmic23

### tlevanic23

## Upotreba umjetne inteligencije

### kkuzmic23

### tlevanic23

# Sustav za Inventar i Resurse - S.I.R.

## Model rada na projektu
Nastavak rada na projektu iz kolegija RPP.<br>
Oba člana će pohađati FINA akademiju za testiranje.

## Opis projekta
Aplikacija na kojoj radimo namijenjena je za praćenje zaliha u poslovnicama. Glavni korisnici bi bili menadžeri ili voditelji smjena kako bi olakšali praćenje inputa i outputa robe. <br>
Projekt je bio izrađen na kolegiju RPP, korištenjem C#, WPF i EntityFramework + MSSQL.

## Projektni tim

Ime i prezime | E-mail adresa (FOI) | JMBAG | Github korisničko ime
------------  | ------------------- | ----- | ---------------------
Karlo Kuzmić | kkuzmic23@foi.hr | 0016165158 | kkuzmic23
Tibor Levanić | tlevanic23@foi.hr | 0016165686 | tlevanic23

## Specifikacija projekta
Na RPP projektu imali smo trećeg člana koji je imao svoj dio funkcionalnih zahtjeva.<br>
On nije upisao ovaj kolegij pa ćemo mi samo nastaviti rad na svojim djelovima.

Oznaka | Naziv | Kratki opis | Odgovorni član tima
------ | ----- | ----------- | -------------------
F01 | Login | Program omogućava prijavu korisnika uz provjeru vjerodajnica, čime se osigurava da samo ovlašteni menadžeri i voditelji smjene mogu pristupiti sustavu i njegovim funkcionalnostima. | Karlo Kuzmić
F02 | Upravljanje narudžbama | Program omogućava unos, pregled i praćenje narudžbi prema dobavljačima. Korisnik može označiti je li narudžba ispunjena i je li roba isporučena, čime se osigurava bolja organizacija nabave bez utjecaja na stanje zaliha. | Karlo Kuzmić
F03 | Upravljanje proizvodima | Program omogućava dodavanje, uređivanje, brisanje i pregled proizvoda koji se prodaju u poslovnici, uključujući osnovne informacije poput naziva, tipa i opisa proizvoda. | Karlo Kuzmić
F04 | Upravljanje partnerskim poduzećima | Program omogućava dodavanje, uređivanje, brisanje i pregled informacija o poduzećima s kojima poslovnica surađuje. Korisnik može evidentirati kontaktne podatke, tip  i uvjete suradnje sa poduzećem. | Karlo Kuzmić
F05 | Upravljanje uvozom robe | Program omogućava unos informacija o uvozu proizvoda, uključujući količinu i datum unosa. Sustav automatski ažurira stanje zaliha i stvara bilješku o uvozu. | Tibor Levanić
F06 | Prikaz zaliha | Program omogućava prikaz trenutnog stanja zaliha za sve proizvode u poslovnici. Korisnik u svakom trenutku može pregledati količine dostupnih artikala i njihovu ažuriranu evidenciju.  | Tibor Levanić
F07 | Upravljanje izvozom robe | Program omogućava unos podataka o dnevnom izvozu proizvoda, uključujući prodaju, veleprodaju, gubitke zbog krađe ili isteka roka trajanja. Svaki unos automatski smanjuje stanje zaliha i stvara bilješku o izvozu. | Tibor Levanić

## Tehnologije i oprema
Za projektiranje sustava ponajprije će biti korišten Visual Paradigm. Program će biti Windows Forms aplikacija razvijena pomoću .NET Framework razvojnog okvira. Za verzioniranje programskog koda bit će korišten GitHub, dok će dokumentacija biti pisana u GitHub Wiki. Projektni zadatak će biti planiran u alatu GitHub projects.
