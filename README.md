# Sustav Inventar i Resurse - S.I.R.
(u redak iznad navedite kratki proizvoljni naziv projekta prikladan akademskoj zajednici, a ovaj tekst obrišite)

## Projektni tim

Ime i prezime | E-mail adresa (FOI) | JMBAG | Github korisničko ime
------------  | ------------------- | ----- | ---------------------
Karlo Kuzmić | kkuzmic23@foi.hr | 0016165158 | kkuzmic23
Tibor Levanić | tlevanic23@foi.hr | 0016165686 | tlevanic23
Tin Račić | tracic21@foi.hr | 0016154766 | tracic21

## Opis domene
Zadatak projekta je izraditi programsko rješenje namijenjeno praćenju zaliha u poduzeću koje prodaje proizvode. Program bi uglavnom koristili menadžeri poslovnica ili upravitelji smjena kako bi se povećala učinkovitost poslovanja i olakšalo upravljanje robom. Program će sadržavati različite funkcionalnosti koje omogućuju praćenje zaliha, evidentiranje uvoz i izvoz robe, stvaranje izvještaja te automatsko obavještavanje ako će ponestat zaliha nekog proizvoda. Korisnici bi na računalu morali jednostavno upravljati podacima o proizvodima, zalihama i partnerskim poduzećima. 

## Specifikacija projekta

Oznaka | Naziv | Kratki opis | Odgovorni član tima
------ | ----- | ----------- | -------------------
F01 | Upravljanje narudžbama | Program omogućava unos, pregled i praćenje narudžbi prema dobavljačima. Korisnik može označiti je li narudžba ispunjena i je li roba isporučena, čime se osigurava bolja organizacija nabave bez utjecaja na stanje zaliha. | Karlo Kuzmić
F02 | Upravljanje proizvodima | Program omogućava dodavanje, uređivanje, brisanje i pregled proizvoda koji se prodaju u poslovnici, uključujući osnovne informacije poput naziva, tipa i opisa proizvoda. | Karlo Kuzmić
F03 | Upravljanje partnerskim poduzećima | Program omogućava dodavanje, uređivanje, brisanje i pregled informacija o poduzećima s kojima poslovnica surađuje. Korisnik može evidentirati kontaktne podatke, tip  i uvjete suradnje sa poduzećem. | Karlo Kuzmić
F04 | Upravljanje uvozom robe | Program omogućava unos informacija o uvozu proizvoda, uključujući količinu i datum unosa. Sustav automatski ažurira stanje zaliha i stvara bilješku o uvozu. | Tibor Levanić
F05 | Prikaz zaliha | Program omogućava prikaz trenutnog stanja zaliha za sve proizvode u poslovnici. Korisnik u svakom trenutku može pregledati količine dostupnih artikala i njihovu ažuriranu evidenciju.  | Tibor Levanić
F06 | Upravljanje izvozom robe | Program omogućava unos podataka o dnevnom izvozu proizvoda, uključujući prodaju, veleprodaju, gubitke zbog krađe ili isteka roka trajanja. Svaki unos automatski smanjuje stanje zaliha i stvara bilješku o izvozu. | Tibor Levanić
F07 | Sustav obavijesti | Program omogućava automatsko slanje obavijesti korisniku kada zaliha nekog proizvoda padne ispod unaprijed definiranog minimuma, i to putem aplikacijskog upozorenja i e-mail poruke. | Tin Račić
F08 | Statistika | Program omogućava prikaz statističkih podataka o prometu i stanju poslovanja, uključujući analizu najprodavanijih proizvoda, prosječnu vrijednost izvoza po danu te sezonske trendove prodaje. | Tin Račuć
F09 | Stvaranje izvještaja | Program omogućava generiranje periodičnih izvještaja o poslovanju, koji uključuju podatke o zalihama, uvozu, izvozu i financijskim pokazateljima, radi donošenja informiranih menadžerskih odluka. | Tin Račić

## Tehnologije i oprema
Za projektiranje sustava ponajprije će biti korišten Visual Paradigm. Program će biti Windows Forms aplikacija razvijena pomoću .NET Framework razvojnog okvira. Za verzioniranje programskog koda bit će korišten GitHub, dok će dokumentacija biti pisana u GitHub Wiki. Projektni zadatak će biti planiran u alatu GitHub projects.
