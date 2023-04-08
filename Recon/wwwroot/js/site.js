// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function IntervalCalculate(data) {


        const groupedData = data.reduce((acc, cur) => {
            const date = new Date(cur.date).toLocaleDateString();
            const time = new Date(cur.date).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit', hour12: false });

            if (!acc[date]) {
                acc[date] = [{ start: time, end: time }];
            } else {
                const last = acc[date][acc[date].length - 1];
                if (last.end === time) {
                    last.end = time;
                } else {
                    acc[date].push({ start: time, end: time });
                }
            }

            return acc;
        }, {});

        const intervalsByDay = Object.entries(groupedData).reduce((acc, [date, times]) => {
            const intervals = times.map((time, i) => {
                const start = new Date(`${date} ${time.start}`);
                const end = new Date(`${date} ${time.end}`);
                return { start, end };
            });

            acc[date] = [];
            for (let i = 0; i < intervals.length - 1; i++) {
                acc[date].push({ start: intervals[i].end.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit', hour12: false }), end: intervals[i + 1].start.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit', hour12: false }) });
            }

            return acc;
        }, {});



        const earliestLatestByDay = Object.entries(intervalsByDay).reduce((acc, [date, intervals]) => {
            var times = intervals.reduce((times, interval) => {
                times.push(interval.start, interval.end);
                return times;
            }, []);


            const uniqueTimes = [...new Set(times)];

            uniqueTimes.sort();


            const linearIntervals = [];

            for (let i = 0; i < uniqueTimes.length - 1; i++) {

                const [month, day, year] = date.split('/');
                const paddedMonth = month.padStart(2, '0');
                const paddedDay = day.padStart(2, '0');

                const start = new Date(`${year}-${paddedMonth}-${paddedDay}T${uniqueTimes[i]}`);
                const end = new Date(`${year}-${paddedMonth}-${paddedDay}T${uniqueTimes[i + 1]}`);

                linearIntervals.push({
                    start: start.toLocaleTimeString([], {
                        hour: '2-digit',
                        minute: '2-digit',
                        hour12: false
                    }),
                    end: end.toLocaleTimeString([], {
                        hour: '2-digit',
                        minute: '2-digit',
                        hour12: false
                    })
                });
            }

            acc[date] = linearIntervals;
            return acc;
        }, {});



        const output = [];
        var workedHours = 0;
        var workedMinutes = 0;

        var BreakHours = 0;
        var BreakMinutes = 0;
        Object.keys(earliestLatestByDay).forEach(date => {
            var counter = 0;
            earliestLatestByDay[date].forEach(interval => {
                const [startHour, startMinute] = interval.start.split(':');
                const [endHour, endMinute] = interval.end.split(':');
                const startDate = new Date(`$2022-02-01 ${startHour}:${startMinute}`);
                const endDate = new Date(`2022-02-01 ${endHour}:${endMinute}`);
                //console.log(startDate)
                //console.log(endDate)
                //console.log(counter)
                
                if (counter % 2 == 0) {

                    workedHours += (endHour - startHour)
                    workedMinutes += (endMinute - startMinute)
                    output.push({
                        date,
                        start: startDate,
                        end: endDate,
                        type: 'Irodában'
                    });
                } else {
                    BreakHours += (endHour - startHour)
                    BreakMinutes += (endMinute - startMinute)
                    output.push({
                        date,
                        start: startDate,
                        end: endDate,
                        type: 'Szünet'
                    });
                }

                // }
                counter++;

            });
        });

      
        //console.log(BreakHours)
        if (BreakMinutes < 0) {
            //console.log("IF" + BreakHours)
            let mod = Math.ceil((BreakMinutes / 60));
            //console.log("MODD" + mod)

            //console.log("IF2" + BreakHours)
            if (mod == 0) {
                BreakHours = BreakHours - 1;
               // console.log(BreakHours)
                BreakMinutes += 60
            } else {

            }
        }


        $("#worked-label").text(workedHours + " óra " + workedMinutes + " perc");
        $("#break-label").text(BreakHours + " óra " + BreakMinutes + " perc");
        chartInstance.option('dataSource', output);
        //console.log(output);

    }