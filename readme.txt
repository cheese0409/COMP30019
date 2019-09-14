Description:
To create the terrain, we used diamond-square algorithm, starting from initializing heights for four corners and for each diamond and square steps, we calculated the average height from four points and added a random noise in order to make the terrain realistic. In addition, we set different colors according to the height of the mountains. To make it realistic under the sunlights, we applied ambient and diffuse reflection on the hill.

In the water parts, we represented the water using a simple plane that go through the mountain. In the water shader, we applied wave effect and performed ambient, diffuse and specular reflections on the water.



Resources: 
Parts of the diamond-square algorithm are retrieved from website https://www.youtube.com/watch?v=1HV8GbFnCik, the method to create the landscape using the general idea of the source code from that link, and modified to suit this project.

Most of the shaders, including the phong shader for the terrain and water shader, are retrieved from the COMP30019 workshop source code materials to complete this project.

