# Inicijalne upute za prijavu projekta iz Razvoja programskih proizvoda

Poštovane kolegice i kolege, 

čestitamo vam jer ste uspješno prijavili svoj projektni tim na kolegiju Razvoj programskih proizvoda, te je za vas automatski kreiran repozitorij koji ćete koristiti za verzioniranje vašega koda, ali i za pisanje dokumentacije.

Ovaj dokument (README.md) predstavlja **osobnu iskaznicu vašeg projekta**. Vaš prvi zadatak je **prijaviti vlastiti projektni prijedlog** na način da ćete prijavu vašeg projekta, sukladno uputama danim u ovom tekstu, napisati upravo u ovaj dokument, umjesto ovoga teksta.

Za upute o sintaksi koju možete koristiti u ovom dokumentu i kod pisanje vaše projektne dokumentacije pogledajte [ovaj link](https://guides.github.com/features/mastering-markdown/).
Sav programski kod potrebno je verzionirati u glavnoj **master** grani i **obvezno** smjestiti u mapu Software. Sve artefakte (npr. slike) koje ćete koristiti u vašoj dokumentaciju obvezno verzionirati u posebnoj grani koja je već kreirana i koja se naziva **master-docs** i smjestiti u mapu Documentation.

Nakon vaše prijave bit će vam dodijeljen mentor s kojim ćete tijekom semestra raditi na ovom projektu. Mentor će vam slati povratne informacije kroz sekciju Discussions također dostupnu na GitHubu vašeg projekta. A sada, vrijeme je da prijavite vaš projekt. Za prijavu vašeg projektnog prijedloga molimo vas koristite **predložak** koji je naveden u nastavku, a započnite tako da kliknete na *olovku* u desnom gornjem kutu ovoga dokumenta :) 

# SIR - Sustav za Inventar i Resurse
(u redak iznad navedite kratki proizvoljni naziv projekta prikladan akademskoj zajednici, a ovaj tekst obrišite)

## Projektni tim

Ime i prezime | E-mail adresa (FOI) | JMBAG | Github korisničko ime
------------  | ------------------- | ----- | ---------------------
Karlo Kuzmić | kkuzmic23@foi.hr | 0016165158 | kkuzmic23
Tibor Levanić | tlevanic23@foi.hr | 0016165686 | tlevanic23
Tin Račić | ... | ... | ...

## Opis domene
Zadatak projekta je izraditi programsko rješenje namijenjeno praćenju zaliha u poduzeću koje prodaje proizvode. Program bi uglavnom koristili menadžeri poslovnica ili upravitelji smjena kako bi se povećala učinkovitost poslovanja i olakšalo upravljanje robom. Program će sadržavati različite funkcionalnosti koje omogućuju praćenje zaliha, evidentiranje uvoz i izvoz robe, stvaranje izvještaja te automatsko obavještavanje ako će ponestat zaliha nekog proizvoda. Korisnici bi na računalu morali jednostavno upravljati podacima o proizvodima, zalihama i partnerskim poduzećima. 

## Specifikacija projekta
Umjesto ovih uputa opišite zahtjeve za funkcionalnošću programskog proizvoda. Pobrojite osnovne funkcionalnosti i za svaku naznačite ime odgovornog člana tima. Opišite buduću arhitekturu programskog proizvoda. Obratite pozornost da bi arhitektura trebala biti višeslojna s odvojenom (dislociranom) bazom podatka koju ćemo za vas mi pripremiti i dati vam pristup naknadno. Također uzmite u obzir da bi svaki član tima treba biti odgovoran za otprilike 3 funkcionalnosti, te da bi opterećenje članova tima trebalo biti ujednačeno. Priložite odgovarajuće dijagrame i skice gdje je to prikladno. Funkcionalnosti sustava bobrojite u tablici ispod koristeći predložak koji slijedi:

Oznaka | Naziv | Kratki opis | Odgovorni član tima
------ | ----- | ----------- | -------------------
F01 | Login | Program omogućava prijavu korisnika uz provjeru vjerodajnica, čime se osigurava da samo ovlašteni menadžeri i voditelji smjene mogu pristupiti sustavu i njegovim funkcionalnostima. | Karlo Kuzmić
F02 | Upravljanje narudžbama | Program omogućava unos, pregled i praćenje narudžbi prema dobavljačima. Korisnik može označiti je li narudžba ispunjena i je li roba isporučena, čime se osigurava bolja organizacija nabave bez utjecaja na stanje zaliha. | Karlo Kuzmić
F03 | Upravljanje proizvodima | Program omogućava dodavanje, uređivanje, brisanje i pregled proizvoda koji se prodaju u poslovnici, uključujući osnovne informacije poput naziva, tipa i opisa proizvoda. | Karlo Kuzmić
F04 | Upravljanje partnerskim poduzećima | Program omogućava dodavanje, uređivanje, brisanje i pregled informacija o poduzećima s kojima poslovnica surađuje. Korisnik može evidentirati kontaktne podatke, tip  i uvjete suradnje sa poduzećem. | Karlo Kuzmić
F05 | Upravljanje uvozom robe | Program omogućava unos informacija o uvozu proizvoda, uključujući količinu i datum unosa. Sustav automatski ažurira stanje zaliha i stvara bilješku o uvozu. | Tibor Levanić
F06 | Prikaz zaliha | Program omogućava prikaz trenutnog stanja zaliha za sve proizvode u poslovnici. Korisnik u svakom trenutku može pregledati količine dostupnih artikala i njihovu ažuriranu evidenciju.  | Tibor Levanić
F07 | Upravljanje izvozom robe | Program omogućava unos podataka o dnevnom izvozu proizvoda, uključujući prodaju, veleprodaju, gubitke zbog krađe ili isteka roka trajanja. Svaki unos automatski smanjuje stanje zaliha i stvara bilješku o izvozu. | Tibor Levanić
F08 | Sustav obavijesti | Program omogućava automatsko slanje obavijesti korisniku kada zaliha nekog proizvoda padne ispod unaprijed definiranog minimuma, i to putem aplikacijskog upozorenja i e-mail poruke. | Tin Račić
F09 | Statistika | Program omogućava prikaz statističkih podataka o prometu i stanju poslovanja, uključujući analizu najprodavanijih proizvoda, prosječnu vrijednost izvoza po danu te sezonske trendove prodaje. | Tin Račuć
F010 | Stvaranje izvještaja | Program omogućava generiranje periodičnih izvještaja o poslovanju, koji uključuju podatke o zalihama, uvozu, izvozu i financijskim pokazateljima, radi donošenja informiranih menadžerskih odluka. | Tin Račić

## Tehnologije i oprema
Za projektiranje sustava ponajprije će biti korišten Visual Paradigm. Program će biti Windows Forms aplikacija razvijena pomoću .NET Framework razvojnog okvira. Za verzioniranje programskog koda bit će korišten GitHub, dok će dokumentacija biti pisana u GitHub Wiki. Projektni zadatak će biti planiran u alatu GitHub projects.
