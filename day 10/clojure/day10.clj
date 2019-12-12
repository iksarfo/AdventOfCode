(require '[clojure.string :as str])

(defn delta-x [fro to]
    (- (:x to) (:x fro)))

(defn delta-y [fro to]
    (- (:y fro) (:y to)))

(defn get-angle [fro to]
    (let [angle (Math/toDegrees (Math/atan2 (delta-x fro to) (delta-y fro to)))]
        (if (< angle 0) (+ angle 360) angle)))

(def example (slurp "<insert-path>\\input.txt"))
(def mapped (str/split example #"\r\n"))

(defn asteroid-locations
    ([map]
        (asteroid-locations map 0 []))

    ([map y found-asteroids]
        (if (empty? map)
            found-asteroids
            (let [next-asteroids (asteroid-locations (first map) 0 y [])]
                (asteroid-locations (rest map) (inc y) (conj found-asteroids next-asteroids)))))

    ([x-axis x y found-asteroids]
        (let [another (str/index-of x-axis "#" x)]
            (if (nil? another)
                found-asteroids
                (recur x-axis (inc another) y (conj found-asteroids [another y]))))))

(defn re-coordinate
    ([input]
        (re-coordinate input []))
    ([input output]
        (if (empty? input) 
            output
            (recur (nthrest input 2) (conj output [(first input) (second input)])))))

(def asteroids (re-coordinate (flatten (asteroid-locations mapped))))

(defn except-self [asteroids asteroid]
    (let [[X Y] asteroid]
        (filter #(or (not= (first %) X) (not= (second %) Y)) asteroids)))

(defn get-asteroid-angles 
    ([asteroid asteroids]
        (get-asteroid-angles asteroid (except-self asteroids asteroid) []))

    ([asteroid asteroids angles]
        (if (empty? asteroids)
            (distinct angles)
            (let [
                    [x1 y1] asteroid
                    [x2 y2] (first asteroids)
                    angle (get-angle {:x x1 :y y1} {:x x2 :y y2})
                ]
                (recur asteroid (rest asteroids) (conj angles angle))))))

(defn visible-asteroids
    ([asteroids]
        (visible-asteroids asteroids 0 {}))

    ([asteroids id sighted-asteroids]
        (if (= id (count asteroids))
            sighted-asteroids
            (let [
                    current-asteroid (nth asteroids id)
                    newly-sighted (count (get-asteroid-angles current-asteroid asteroids))
                ]
                (recur asteroids (inc id) (assoc sighted-asteroids current-asteroid newly-sighted))))))

(def sighted (visible-asteroids asteroids))
(def asteroid-with-best-view (key (apply max-key val sighted)))
(def part-one (get sighted asteroid-with-best-view))
