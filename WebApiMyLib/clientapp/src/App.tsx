import Typography from '@mui/material/Typography';
import {
    BrowserRouter as Router,
    Route,
    Routes,
} from 'react-router-dom';
import { AuthorList } from './components/author-list/author-list';
import { BookList } from './components/book-list/book-list';
import { CategoryList } from './components/category-list/category-list';
import NavTabs from './components/navigation-tabs/navigation-tabs';
import './App.scss';

function App() {
    return (
        <div className="App">

            <Typography
                variant="h3"
                component="div"
                gutterBottom
                align="left">
                My Library
            </Typography>

            <Router>
                <NavTabs />

                <div className="content-container">

                    <Routes>
                        <Route path="/books" element={<BookList />} />

                        <Route path="/authors" element={<AuthorList />} />

                        <Route path="/categories" element={<CategoryList />} />
                    </Routes>
                </div>
            </Router>
        </div>
    );
}

export default App;
