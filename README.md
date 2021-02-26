# 3D-Shape-Editor
Jest to aplikacja pozwalająca dodawać i oglądać kolorowe figury w przestrzeni trójwymiarowej. Została wykonana na zaliczenie przedmiotu na studiach.

![gif](https://media.giphy.com/media/dvF7FYlo9x6G1oD1Ns/giphy.gif)

# Technologia
Aplikacja została napisana w języku C# korzystając z frameworku Windows Forms. Figury sa reprezentowane za pomocą trójkątów a potok renderowania jest zaimplementowany własnoręcznie (zatem aplikacja korzysta z CPU).
# Funkcjonalności
 - Dodawanie nowych prostopadłościanów i kul według ustalonych parametrów przez użytkownika
 - Przełączanie pomiędzy rysowaniem samych krawędzi, wypełnianiem jednolitym kolorem oraz wypełnianiem kolorem z cieniowaniem (Flat Shading lub Phong Shading)
 - Poruszanie kamery w przestrzeni
 - Obcinanie ścian tylnych (Backface culling)
 - Obcinanie trójkątów wystających poza pole widzenia kamery
 - Buforowanie głębi (Z-Buffering)

![image](https://i.imgur.com/IOXiwey.jpg)
