import { type RouteConfig, index, route } from "@react-router/dev/routes";

export default [
    index("routes/home.tsx"),
    route("/projectOverview", "routes/projectOverview.tsx"),
    route("/timeRegistration", "routes/timeRegistration.tsx"),
] satisfies RouteConfig;
