import Box from '@mui/material/Box';
import Tabs from '@mui/material/Tabs';
import Tab from '@mui/material/Tab';
import { Link, useLocation } from 'react-router-dom';

const routes = {
    books: '/books',
    authors: '/authors',
    categories: '/categories',
};

export default function NavTabs() {
    const location = useLocation();

    return (
        <Box sx={{ width: '100%' }}>
            <Tabs value={location.pathname}>

                <Tab
                    label="Books"
                    component={Link}
                    to={routes.books}
                    value={routes.books} />

                <Tab
                    label="Authors"
                    component={Link}
                    to={routes.authors}
                    value={routes.authors} />

                <Tab
                    label="Categories"
                    component={Link}
                    to={routes.categories}
                    value={routes.categories} />
            </Tabs>
        </Box>
    );
}
