/*

INSERT INTO [dbo].[Members](pseudo,role,email,password,salt,bankroll,playing,current_tournament_id,disconnected)VALUES('Gemini','player','gemini@gmail.com',0x6EB6D109AD364206E51278F8D470017C857CDDFEB11E7DC6F1880DF5F883BC25,0x2455C199DCF703F9A1CC718FB17FEF60,15000,0,0,1);
INSERT INTO [dbo].[Members](pseudo,role,email,password,salt,bankroll,playing,current_tournament_id,disconnected)VALUES('Bauman','player','bau@gmail.com',0x6EB6D109AD364206E51278F8D470017C857CDDFEB11E7DC6F1880DF5F883BC25,0x2455C199DCF703F9A1CC718FB17FEF60,15000,0,0,1);
INSERT INTO [dbo].[Members](pseudo,role,email,password,salt,bankroll,playing,current_tournament_id,disconnected)VALUES('calamusa','player','cala@gmail.com',0x6EB6D109AD364206E51278F8D470017C857CDDFEB11E7DC6F1880DF5F883BC25,0x2455C199DCF703F9A1CC718FB17FEF60,15000,0,0,1);
INSERT INTO [dbo].[Members](pseudo,role,email,password,salt,bankroll,playing,current_tournament_id,disconnected)VALUES('simon','player','simon@gmail.com',0x6EB6D109AD364206E51278F8D470017C857CDDFEB11E7DC6F1880DF5F883BC25,0x2455C199DCF703F9A1CC718FB17FEF60,15000,0,0,1);
INSERT INTO [dbo].[Members](pseudo,role,email,password,salt,bankroll,playing,current_tournament_id,disconnected)VALUES('chichi','player','chichi@gmail.com',0x6EB6D109AD364206E51278F8D470017C857CDDFEB11E7DC6F1880DF5F883BC25,0x2455C199DCF703F9A1CC718FB17FEF60,15000,0,0,1);

INSERT INTO [dbo].[Gains](gains_sharing_nr,start_place,end_place,percentage)VALUES(1,1,1,100);
INSERT INTO [dbo].[Gains](gains_sharing_nr,start_place,end_place,percentage)VALUES(2,1,1,70);
INSERT INTO [dbo].[Gains](gains_sharing_nr,start_place,end_place,percentage)VALUES(2,2,2,30);

INSERT INTO [dbo].[Tournaments_types](
    buy_in,
    late_registration_level,
    starting_stack,
    rebuy,
    rebuy_level,
    levels_duration,
    min_players,
    max_players,
    players_per_table,
    max_paid_places,
    gains_sharing_nr
)VALUES(
    2000,
    0,
    1500,
    0,
    0,
    300,
    2,
    2,
    2,
    1,
    1
);
INSERT INTO [dbo].[Tournaments_types](
    buy_in,
    late_registration_level,
    starting_stack,
    rebuy,
    rebuy_level,
    levels_duration,
    min_players,
    max_players,
    players_per_table,
    max_paid_places,
    gains_sharing_nr
)VALUES(
    2000,
    0,
    1500,
    0,
    0,
    300,
    3,
    3,
    3,
    1,
    1
);

INSERT INTO [dbo].[Tournaments_types](
    buy_in,
    late_registration_level,
    starting_stack,
    rebuy,
    rebuy_level,
    levels_duration,
    min_players,
    max_players,
    players_per_table,
    max_paid_places,
    gains_sharing_nr
)VALUES(
    3000,
    0,
    3000,
    0,
    0,
    300,
    2,
    4,
    4,
    2,
    2
);

*/
