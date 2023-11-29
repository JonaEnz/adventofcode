package main

import (
	"bufio"
	"fmt"
	"log"
	"math"
	"os"
	"strconv"
)

func main() {
	f, err := os.Open("day01.txt")
	if err != nil {
		log.Fatal(err)
	}

	defer f.Close()

	scanner := bufio.NewScanner(f)

	sum := 0.0

	for scanner.Scan() {
		fmt.Printf("%s\n", scanner.Text())
		mass, _ := strconv.Atoi(scanner.Text())
		fuel := math.Floor(float64(mass/3)) - 2
		fmt.Printf("%i -> %i\n", mass, fuel)
		sum += fuel
	}
	fmt.Printf("%f", (sum))
}