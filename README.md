# Praksa

3.2.: DOBIVENE INFORMACIJE ZA APP: microsoft tehnologija, .net 4.8 framework, wpf, 10-20 labela?, resorce datoteke?

13.2.: RIVALA SAN ALI VJV ĆE SE NAJ NEŠTO ČA NI DORBO! :|

OPIS PREVOĐENJA:

TranslateResources:

1. Učitava resursnu datoteku Resources.resx (osnovna) i Resources.en.resx (prevedena)

2. Ako prevedena ne postoji, stvara je s osnovnim elementima

3. Prolazi kroz svaku stavku (data element) unutar osnovne datoteke te sprema name i value od svake

4. Provjerava postoji li taj element u prevedenoj datoteci

5. Ako postoji (uz provjeru da je value različit od originala) prekače se naredba linija koda

6. Ako ne postoji poziva se TranslateText za value tog određenog elementa

7. Proverava li se postoji li u prevedenoj datoteci element s tim name-on: ako da onda se samo upisuje value za njega a ako ne onda se stvara novi elemnt s tim name-on i prevedenin value-on

8. Kada se to izvrši za sve elemente, upisuje se u prevedenu datoteku

TranslateText:

1. Definira se putanja fo python izvršne datoteke i do python skripte za prevođenje

2. Definiraju se argumenti koji se proslijeđuju python skripti

3. Definira se proces i pokreće

4. Nakon što se proces završi, ako nema grešaka ispisuje se rezultat te opkrenute skripte
