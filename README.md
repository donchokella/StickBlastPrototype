# StickBlastPrototype
This game is a prototype of Stick Blast 

Day 1

12.00- 13.15	I reviewed the email I received and played the game to get an idea about it. After gaining some insight, I thought about the core mechanics and decided to start by creating a flowchart.

16.00 		I created the flowchart, but at one point I realized I made a critical mistake. I had planned some of the operations based on the nodes instead of the bars. So, I took some time to review the game and made a new plan, then created a new flowchart.

18.30 		I initially thought of creating a script called PieceGenerator to merge the sticks. However, after a while, I realized that this was a pointless effort and decided that manually merging them would be both easier and more logical. This is because there aren’t too many piece types, and they don’t form in a specific pattern.

21.00		I tried merging them manually for a while, but I soon realized that the pieces don’t quite fit together. I had no idea how to solve it. I thought about it for a bit, but since I was spending too much time on this part, I decided to leave it for later and started thinking about how to solve the grid system in the game.

22.00 		I wondered whether I should follow the nodes, the bars, or my cells. In the end, I decided to use the cells.

24.00		By this time, I managed to achieve the following:

			1) Created three pieces (as placeholders) in the selection area.
			2) Enabled the pieces to be dragged with an offset when clicked.
			3) Implemented a faded preview of the piece during dragging (this will be refined so that the fading only occurs in appropriate locations).
			4) Set up the grid (since I built it based on cells, this part might be completely overhauled later; I started it without fully thinking it through because I was very tired).
			5) Enabled the pieces to snap into place.
01.00		Currently, a piece can only be placed on the grid. I manually merged the sprites of the pieces onto the placeholders and turned off the sprite renderers for the placeholders. I ended my work for today so I could think clearly later.
a

Day 2

9.30		I reviewed my most recent work and realized that I made some serious mistakes by making poor decisions. Relying on cells turned out to be a very bad choice. I could continue with this approach, but it would significantly complicate the process, or I could rebuild the system from scratch. Fortunately, I believe that some of my solutions will still work for the alternative option. To avoid repeating yesterday's mistakes, I'll take some time to carefully consider how to proceed before moving forward.

10.00		I decided to change the system because if I continued with the current approach, bigger challenges would arise later. 
		
18.00		Now our system includes nodes, bars, and cells; however, the piece snap mechanism isn’t working correctly. Both horizontal and vertical pieces appear as vertical in the preview. I updated the prefabs to resolve this issue. For now, I built the system only for the "I" shaped piece, which consists of a single stick. It will likely cause problems later, but I thought it was more sensible to start with a simple approach to understand what needs to be done.

21.15		At this point, I started to properly align the "I" shaped pieces to the cell borders. The previews work correctly, but the cell coloring mechanism is a bit off. I need to focus on that. Moving to a bar and node system proved very beneficial.

22.00		Right now, I can color the cells, and if no piece from the selection area can be placed, I get a game over debug message. When rows or columns fill up, I’ve turned off the cell coloring. However, the bars are not being cleared – I need to fix that. I also added a reload button to ease testing.


Day 3

10.00		I compared the prototype with the initial flowchart and went through what has been done and what still needs to be done.

11.10		After blast (row/column clearing) occurs, the cells return to their initial state, but the bars are not behaving as expected. I struggled to find the root cause and decided to try a new approach. In this approach, the placed piece becomes a child of the bar where it is attached. During the blast check, the sticks attached to that cell are inspected and, if there are extra child objects, they are removed.

13.00		I expected this method to completely resolve the issue, but now a small problem remains: the last dragged stick remains on the screen. However, the grid seems to recognize that the area is cleared, meaning a new stick can be placed there. The problem seems to be only with the visual feedback. This can probably be solved by fixing the visual aspect.

15.00 		I reviewed the code thoroughly, but I couldn’t find the exact cause. Initially, I suspected an issue with the method execution order (and I still think it might be related). Perhaps the piece is reparented after the bars are cleared? I investigated and tried to fix it, but it turned out not to be the case.

20.00		While debugging, I discovered a logical error in how the Stick and Piece operations were handled. Most of the dragging and dropping operations were implemented in Piece.cs, while Stick.cs was only used to control the orientation of the sticks. This will likely be problematic when adding pieces with multiple sticks in the future. I need to clearly define the scope of	 Stick.cs and Piece.cs. I believe that the operations related to dragging and releasing the piece should remain in Piece.cs, but it might be more logical for snapping and preview functionalities to be controlled from Stick.cs. This way, managing pieces with multiple sticks will be easier.

Day 4

10.00 		Stick.cs ve Piece.cs scriptlerinin sorumluluklarini duzenlemeye basladim.

12.00		Her bar eklendiginde 5 puan, her hucre doldugunda 15 puan, her blast gerceklestiginde 100 puan alinmakta. belirli bir limiti gecince seviye atlanmakta ve yeni seviyenin limiti %20 artarak guncellenmekte. Fakat barlar yine burda gorsel olarak dolu kalmakta. Bunu cozmeliyim

15.30		Piece ve stick sorumluluklari duzenlendi

17.00		Cok stickli piecler artik oyuna dahil edilebildi, fakat bazi buglar var.







