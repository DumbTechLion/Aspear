import { Chart as ChartJS, ArcElement, PieController, Tooltip, Legend, CategoryScale } from "chart.js";
import ChartDataLabels from 'chartjs-plugin-datalabels';

export default defineNuxtPlugin(() => {
  ChartJS.register(ArcElement, PieController, Tooltip, Legend, CategoryScale, ChartDataLabels);
});
