# Praksa

3.2.: DOBIVENE INFORMACIJE ZA APP: microsoft tehnologija, .net 4.8 framework, wpf, 10-20 labela?, resorce datoteke?

13.2.: RIVALA SAN ALI VJV ĆE SE NAJ NEŠTO ČA NI DORBO! :|

OPIS PREVOĐENJA:

TranslateResources:

1. Učitava resursnu datoteku Resources.resx i Resources.en.resx 

2. Provjerava svaku stavku (data element) unutar resursa

3. Ako se određeni tekst još nije preveo, poziva metodu TranslateText() da prevede taj tekst

4. Pohranjuje prevedene vrijednosti natrag u Resources.en.resx

TranslateText:

1. Pokreće Python skriptu pomoću Process.Start()

2. Predaje joj ulazni tekst i jezike za prijevod

3. Vraća prevedeni tekst ili prikazuje grešku ako nešto pođe po zlu
