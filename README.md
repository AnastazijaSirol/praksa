# Praksa

DOBIVENE INFORMACIJE ZA APP: microsoft tehnologija, .net 4.8 framework, wpf, 10-20 labela?, resorce datoteke?

17.2.

MISLIN DA JE TO TO ČA SE TIČE APLIKACIJE! 

- sada triba provuć njihov file kroz to

- umjesto da se šalje tekst po tekst sada se šalje cijeli file na prijevod pa je brže

OPIS:

TranslateResources()

- definiraju se putanje do 3 file-a: izvorni, prevedeni i još uvijek neprevedeni

- učitava se file sa izvornim tekstovima 

- ako prevedni file ne postoji, stvara se novi s osnovnim elementima i sprema na definiranu putanju

- učitava se file sa prevednim tesktovima

- traže se resursi koji nisu prevedeni (nalaze se u izvornom file-u, ali ne i u prevednom)

- ako ne postoje takvi resursi funkcija se prekida, a ako postoje oni se spremaju u file još neprevedenih tekstova na definiranoj putanji

- poziva se funkcija koja prevodi sadržaj u file-u s neprevedenim resursima

- prolazi se kroz prevedeni file i dodaju se oni resursi koji ne postoje u njemu a nalaze se u još neprevedenom file-u

- ažurirani file se sprema na definiranoj putanji prevedenog file-a

TranslateText()

- učitavaju se: python izvršna datoteka i python skripta za prevođenje 

- pokreće se python skipta

- čita se rezultat koji vraća python skripta nakon što se izvrši

*koristi se model: facebook/m2m100_1.2B sa HuggingFace-a*

*model se vrti lokalno*
