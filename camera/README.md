Dwie metody utrzymywania dwóch graczy w jednym widoku, w obu przypadkach kamera 
podąża za punktem po środku odcinka wyznaczanego przez graczy.
1) "Walls" - przezroczyste ściany przesuwające się z kamerą i oddziałujące jedynie na graczy
2) "Edges" - na podstawie pozycji graczy wewnątrz widoku, zabraniamy graczom poruszać się poza jego obszar, 
zerując odpowiednie składowe prędkości i ignorując żądania ruchu w nieodpowiednią stronę 
