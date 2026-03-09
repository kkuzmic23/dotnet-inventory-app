# Sustav za Inventar i Resurse - S.I.R.

## Model rada na projektu
Nastavak rada na projektu iz kolegija RPP.<br>

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
