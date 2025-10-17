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
Zadatak projekta je izraditi programsko rješenje namijenjeno praćenju zaliha u poduzeću koje prodaje proizvode. Program bi uglavnom koristili menadžeri poslovnica ili upravitelji smjena kako bi se povećala učinkovitost poslovanja i olakšalo upravljanje robom. Program će sadržavati sljedeće funkcionalnosti: login, evidentiranje robe, naručivanje i izvoz robe, upravljanje radnicima i radnim smjenama, stvaranje izvještaja, generiranje statistike, praćenje zaliha te obaviještavanje.  

## Specifikacija projekta
Umjesto ovih uputa opišite zahtjeve za funkcionalnošću programskog proizvoda. Pobrojite osnovne funkcionalnosti i za svaku naznačite ime odgovornog člana tima. Opišite buduću arhitekturu programskog proizvoda. Obratite pozornost da bi arhitektura trebala biti višeslojna s odvojenom (dislociranom) bazom podatka koju ćemo za vas mi pripremiti i dati vam pristup naknadno. Također uzmite u obzir da bi svaki član tima treba biti odgovoran za otprilike 3 funkcionalnosti, te da bi opterećenje članova tima trebalo biti ujednačeno. Priložite odgovarajuće dijagrame i skice gdje je to prikladno. Funkcionalnosti sustava bobrojite u tablici ispod koristeći predložak koji slijedi:

Oznaka | Naziv | Kratki opis | Odgovorni član tima
------ | ----- | ----------- | -------------------
F01 | Login | Za pristup programu potrebna je kombinacija odgovarajućeg korisničkog imena i lozinke. Sustav implementira više tipova korisnika, pri čemu svaki tip ima pristup određenim funkcionalnostima. | Karlo Kuzmić
F02 | Upravljanje naručivanja proizvoda | Program implementira više načina naručivanja proizvoda. Moguće je ručno upisati vrstu i količinu porizvoda za naručivanje, a moguće je i automatsko naručivanje prema određenim parametrima (broj proizvoda na zalihi, broj proizvoda na policama, rok trajanja, trend kupovine, sezonski utjecaji) | Karlo Kuzmić
F03 | Upravljanje zalihama | Cilj sustava je pratiti broj proizvoda na zalihama i osigurati minimalnu moguću zalihu koja i dalje osigurava dovoljan broj proizvoda na policama. Preko programa moguće vidjeti točne vrste i broj proizvoda trenutno na zalihi. Sustav zaliha komunicira sa sustavom za upravljanje narudžbama i sustavom za veleprodaju zbog ostvarenja cilja. | Karlo Kuzmić
F04 | Upravljanje izvozom robe | Program omogućuje veleprodaju, odnosno mogućnost prodaje proizvoda na veliko iz poslovnice drugim partnerima | Tibor Levanić
F05 | Statistika | Generiranje pisanih ili digitalnih izvještaja funkcionalnosti nekih drugih sustava u točnom trenutku u svrhu dokaza ili očitovanja, generiranje statistike kao pomoć u odlučivanju (najprodavaniji proizvod, proizvod s najviše žalbi, graf broja prodanih porizvoda kroz godinu itd.)  | Tibor Levanić
F06 | Obaviještavanje | Sustav omogućuje obaviještavanje korisnika putem kartice "Obavijesti" kada se u sustavu ispuni definirani uvjet (npr. kad na zalihi ima manje od X proizvoda, kada) | Tibor Levanić
F07 |  |  | Tin Račić
F08 | 
F09 | Upravljanje radnicima i radnim smjenama | U sustav je dostupan pregled zapisanih radnika po smjenama kroz tjedne, upis novih radnika, mijenjanje i brisanje smjene, mijenjanje repozitorija radnika | Tin Račić

## Tehnologije i oprema
Za projektiranje sustava ponajprije će biti korišten {PROGRAM ZA UML DIJAGRAME} Program će biti Windows Forms aplikacija razvijena pomoću .NET Framework razvojnog okvira.
Umjesto ovih uputa jasno popišite sve tehnologije, alate i opremu koju ćete koristiti pri implementaciji vašeg rješenja. Projekti se razvijaju koristeći .Net Framework ili .Net Core razvojne okvire, a vrsta projekta može biti WinForms, WPF i UWP. Ne zaboravite planirati korištenje tehnologija u aktivnostima kao što su projektni menadžment ili priprema dokumentacije. Tehnologije koje ćete koristiti bi trebale biti javno dostupne, a ako ih ne budemo obrađivali na vježbama u vašoj dokumentaciji ćete morati navesti način preuzimanja, instaliranja i korištenja onih tehnologija koje su neopbodne kako bi se vaš programski proizvod preveo i pokrenuo. Pazite da svi alati koje ćete koristiti moraju imati odgovarajuću licencu. Što se tiče zahtjeva nastavnika, obvezno je koristiti git i GitHub za verzioniranje programskog koda, GitHub Wiki za pisanje tehničke i projektne dokumentacije, a projektne zadatke je potrebno planirati i pratiti u alatu GitHub projects. 
