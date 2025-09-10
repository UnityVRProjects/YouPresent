## YouPresent

YouPresent is a Unity VR project aimed at helping people gain confidence in their presentation and public speaking skills by simulating a presentation environment in VR. We leveraged Adobe Mixamo to create a randomly generated audience that applauds after finishing the presentation and a coach that gives the user feedback on his presentation. The coach gives the user feedback based on the user's gaze and hand motions, and is sent to Google Gemini to process and give feedback on.

### Features
- A full auditorium scene with a stage area and a presentation deck behind the podium for the user to display the presentation slides if needed.
- An interactive podium that has a menu for setting up the presentation, with the option of a practice run without the coaching and a training option with the coaching.
- A camera that records the user from the audience's perspectve when he starts presenting in training mode and stops when he ends it. The user can see a Mixamo model of himself in the recording when he presents, and views it after he ends the training mode.
- A randomly generated audience that interacts with the user when he ends the presentation, and a coach that gives the user feedback on the his presentation
