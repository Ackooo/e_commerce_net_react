import { Container, Divider, Paper, Typography } from "@mui/material";
import { useLocation } from "react-router-dom";
export default function ServerError() {
  const { state } = useLocation();
  return (
    <Container component={Paper}>
      {state?.error ? (
        <>
          <Typography gutterBottom variant="h3" color="secondary">
            {state.error.title}
          </Typography>
          <Divider />
          {/* producion doest have stack trace so default text */}
          {/* zadrzava se i nakon refresh page */}
          <Typography variant="body1">
            {state.error.detail || "Internal server error"}
          </Typography>
        </>
      ) : (
        <Typography gutterBottom variant="h5">
          Server Error
        </Typography>
      )}
    </Container>
  );
}

