import os
from random import randrange
from datetime import timedelta
from datetime import datetime


query_template = "INSERT INTO player_stats VALUES (%d, %d, '%s', '%s');"
start_date = datetime(2021, 1, 1, 0, 0, 0)
end_date = datetime(2021, 2, 28, 23, 59, 59)
output_query_data = ""

def random_date_range(start, end):
    delta = end - start
    int_delta = (delta.days * 24 * 60 * 60) + delta.seconds
    random_second = randrange(int_delta)
    return start + timedelta(seconds=random_second)


for i in range(200):
    session_start = random_date_range(start_date, end_date)
    play_time = randrange(30 * 60, 180 * 60)
    session_end = session_start + timedelta(seconds=play_time)
    data = query_template % (randrange(1,21), randrange(5000), str(session_start), str(session_end))
    output_query_data += (data + '\n')


with open("generated_player_data.sql", "w+") as file:
    file.write(output_query_data)
